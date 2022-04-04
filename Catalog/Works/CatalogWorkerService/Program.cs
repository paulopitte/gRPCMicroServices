using CatalogWorkerService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddTransient<CatalogFactory>();
    })
    .Build();

await host.RunAsync();
