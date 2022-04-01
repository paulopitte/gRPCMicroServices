using Google.Protobuf.WellKnownTypes;
using GrpcCatalog.Domain;
using GrpcCatalog.Protos;
using System.Data.Entity;

namespace GrpcCatalog.Data
{
    public class CatalogRepository : ICatalogRepository
    {

        private readonly CatalogDbContext _context;
        private ILogger<CatalogRepository> _logger;

        public CatalogRepository(CatalogDbContext context, ILogger<CatalogRepository> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Product> Add(Product product)
        {          
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;

        }

        public async Task<ProductModel> GetProduct(int id)
        {
            var p = await _context.Products.FirstOrDefaultAsync(x=>x.Id == id);
            if (p is null)
                return null;

            return new ProductModel()
            {
                Id = p.Id,
                Sku = p.Sku,
                Title = p.Title,
                Price = p.Price,
                StatusProduct =  Protos.StatusProduct.Actived,
                CreateAt = Timestamp.FromDateTime(p.CreateAt)
            };
        }


        public async Task<List<ProductModel>> GetProducts()
        {
            return _context.Products.Select(
                p => new ProductModel()
                {
                    Id = p.Id,
                    Sku = p.Sku,
                    Title = p.Title,
                    Price = p.Price,
                    StatusProduct = Protos.StatusProduct.Actived,
                    CreateAt = Timestamp.FromDateTime(p.CreateAt)
                }).ToList();
        }
    }

    public interface ICatalogRepository
    {
        Task<List<ProductModel>> GetProducts();
        Task<ProductModel> GetProduct(int id);


        Task<Product> Add(Product product); 
    }
}
