using eGranjaCAT.Application.Entities;
using eGranjaCAT.Domain.Entities;
using eGranjaCAT.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace eGranjaCAT.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Farm> Farms { get; set; }
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Entrada> Entrades { get; set; }
        public DbSet<Visita> Visites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Lot>()
                .HasOne(l => l.Farm)
                .WithMany()
                .HasForeignKey(l => l.FarmId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Entrada>()
                .HasOne(l => l.Farm)
                .WithMany()
                .HasForeignKey(l => l.FarmId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entrada>()
                .HasOne(e => e.Lot)
                .WithMany()
                .HasForeignKey(e => e.LotId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Visita>()
                .HasOne(l => l.Farm)
                .WithMany()
                .HasForeignKey(l => l.FarmId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}