using Challenge.Balta.IBGE.Infra.Mapping;
using Chanllenge.Balta.IBGE.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Challenge.Balta.IBGE.Domain.Entities;

namespace Challenge.Balta.IBGE.Infra.Data.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Ibge> Ibge { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<FileImport> FileImports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ibge>(new IbgeMap().Configure);
            modelBuilder.Entity<ApplicationUser>(new ApplicationUserMap().Configure);
            modelBuilder.Entity<FileImport>(new FileImportMap().Configure);
        }
    }
}
