using eGranjaCAT.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;


namespace eGranjaCAT.Infrastructure.Services
{
    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext _context;

        public StatsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<int> CountTotalRecordsAsync()
        {
            int total = 0;
            var dbSets = _context.GetType().GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProp in dbSets)
            {
                var entityType = dbSetProp.PropertyType.GetGenericArguments().First();
                var dbSet = dbSetProp.GetValue(_context);

                if (dbSet is IQueryable queryable)
                {
                    var method = typeof(EntityFrameworkQueryableExtensions)
                        .GetMethods(BindingFlags.Public | BindingFlags.Static)
                        .First(m => m.Name == nameof(EntityFrameworkQueryableExtensions.CountAsync)
                                    && m.GetParameters().Length == 2)
                        .MakeGenericMethod(entityType);

                    var task = (Task<int>)method.Invoke(null, new object[] { queryable, CancellationToken.None });
                    var count = await task;

                    total += count;
                }
            }

            return total;
        }


        public async Task<int> CountRecentRecordsAsync(int days)
        {
            int total = 0;
            var cutoff = DateTime.UtcNow.AddDays(-days);

            var dbSets = _context.GetType().GetProperties().Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var dbSetProp in dbSets)
            {
                var entityType = dbSetProp.PropertyType.GetGenericArguments().First();
                var createdAtProp = entityType.GetProperty("CreatedAt");

                if (createdAtProp != null)
                {
                    var dbSet = dbSetProp.GetValue(_context) as IQueryable;
                    if (dbSet != null)
                    {
                        var param = Expression.Parameter(entityType, "e");
                        var body = Expression.GreaterThanOrEqual(Expression.Property(param, createdAtProp), Expression.Constant(cutoff));
                        var lambda = Expression.Lambda(body, param);

                        var whereMethod = typeof(Queryable).GetMethods()
                            .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                            .MakeGenericMethod(entityType);

                        var filtered = (IQueryable)whereMethod.Invoke(null, new object[] { dbSet, lambda });

                        var countMethod = typeof(Queryable).GetMethods()
                            .First(m => m.Name == "Count" && m.GetParameters().Length == 1)
                            .MakeGenericMethod(entityType);

                        var count = (int)countMethod.Invoke(null, new object[] { filtered });
                        total += count;
                    }
                }
            }

            return total;
        }


        public async Task<int> CountTotalFarmsAsync()
        {
            return await _context.Farms.CountAsync();
        }

        public async Task<int> CountTotalActiveLotsAsync()
        {
            return await _context.Lots.Where(l => l.Active).CountAsync();
        }

        public async Task<int> CountTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }
    }
}
