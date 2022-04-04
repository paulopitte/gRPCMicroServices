using Grpc.Net.Client;
using GrpcCatalog.Protos;
 

using static System.Console;
namespace CatalogWorkerService
{
    public class CatalogFactory
    {

        private readonly ILogger<CatalogFactory> _logger;
        private readonly IConfiguration _configuration;
        private string SERVER_GRPC = String.Empty;

        public CatalogFactory(ILogger<CatalogFactory> logger, IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            SERVER_GRPC = _configuration.GetValue<string>("WorkerService:ServerUrl");

        }



        public async Task GetProductAsync()
        {
            var client = GetClient();

            WriteLine("GetProductasync started....");

            var response = client.GetProduct(new GetProductRequest { Id = 1 });

            WriteLine($"GetProductasync response: {response.ToString()}");
        }

        private ProductProdtService.ProductProdtServiceClient GetClient()
        {
            if (!SERVER_GRPC.StartsWith("https"))
            {
                AppContext.SetSwitch(
                    "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            }

            var channel = GrpcChannel.ForAddress(SERVER_GRPC);
            var client = new ProductProdtService.ProductProdtServiceClient(channel);
            return client;
        }


    }



}
