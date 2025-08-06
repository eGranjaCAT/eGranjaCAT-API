using eGranjaCAT.Application.Mappings;
using eGranjaCAT.Infrastructure.Identity;
using System.Reflection;


namespace eGranjaCAT.Api.Extensions
{
    public static class MappingExtensions
    {
        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly(),
                Assembly.GetAssembly(typeof(FarmProfile)),
                Assembly.GetAssembly(typeof(LotProfile)),
                Assembly.GetAssembly(typeof(EntradaProfile)),
                Assembly.GetAssembly(typeof(UserProfile))
            );

            return services;
        }
    }
}