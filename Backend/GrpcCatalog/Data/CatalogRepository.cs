using Google.Protobuf.WellKnownTypes;
using GrpcCatalog.Domain;
using GrpcCatalog.Protos;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

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


        public async Task<Product> Update(Product product)
        {
            //var p = _context.Products.AsNoTracking().FirstOrDefault(x => x.Id == product.Id);
            //if (p is null)
            //    return null;

            _context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "OPSS erro ocorrido ");
                return null;
            }
            return product;

        }


        public async Task<int> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception();
            }
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }



        public async Task<ProductModel> GetProduct(int id)
        {
            try
            {
                var p =  _context.Products.AsNoTracking().FirstOrDefault(x=>x.Id ==  id);
                if (p is null)
                    return null;

                return new ProductModel()
                {
                    Id = p.Id,
                    Sku = p.Sku,
                    Title = p.Title,
                    Price = p.Price,
                    StatusProduct = Protos.StatusProduct.Actived,
                    CreateAt = Timestamp.FromDateTime(p.CreateAt)
                };
            }
            catch (Exception)
            {
                _logger.LogError($"Product {id} is not found.");
                return null;
            }
            return null;
            
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
        Task<Product> Update(Product product);
        Task<int> Delete(int product);
    }
}
