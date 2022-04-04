using Grpc.Net.Client;
using GrpcCatalog.Protos;
using static System.Console;

namespace CatalogWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private readonly CatalogFactory _catalogFactory;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, CatalogFactory catalogFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _catalogFactory = catalogFactory ?? throw new ArgumentNullException(nameof(catalogFactory));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {


            WriteLine("Waiting for service is running....");
            Task.Delay(TimeSpan.FromSeconds(5));

            while (!stoppingToken.IsCancellationRequested)
            {

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);


                await _catalogFactory.GetProductAsync().ConfigureAwait(false);

                await Task.Delay(_configuration.GetValue<int>("WorkerService:TaskInterval"), stoppingToken);
            }
        }

    }
}