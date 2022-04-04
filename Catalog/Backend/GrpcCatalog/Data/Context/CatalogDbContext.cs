using GrpcCatalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace GrpcCatalog.Data
{
    public sealed class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        { 
        
        }


        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasKey(x => x.Id);
        }
    }
}
