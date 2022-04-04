using GrpcCatalog.Domain;

namespace GrpcCatalog.Data
{
    public class CatalogDbContextSeed
    {

        public static void SeedAsync(CatalogDbContext catalogDbContext)
        {
            if (catalogDbContext.Products.Any() is false)
            {
                var products = new List<Product>
                {
                    new Product
                    {
                        Sku = "SKU1",
                        Title = "IPhone 14 100Gb",
                        Price = 1_000_000,
                        Status =    StatusProduct.Actived,
                        CreateAt = DateTime.UtcNow.AddHours(-3),
                    },
                    new Product
                    {
                        Sku = "SKU2",
                        Title = "IPhone 15 200Gb",
                        Price = 2_000_000,
                        Status =    StatusProduct.Actived,
                        CreateAt = DateTime.UtcNow.AddHours(-3),
                    },
                    new Product
                    {
                        Sku = "SKU3",
                        Title = "IPhone 16 300Gb",
                        Price = 3_000_000,
                        Status =    StatusProduct.Actived,
                        CreateAt = DateTime.UtcNow.AddHours(-3),
                    }
                };

                catalogDbContext.Products.AddRange(products);
                catalogDbContext.SaveChangesAsync();
            }
        }
    }
}
