using GrpcCatalog.Data;
using GrpcCatalog.Mapper;
using GrpcCatalog.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICatalogRepository, CatalogRepository>();



// Configurando o acesso a dados de produtos
builder.Services.AddDbContext<CatalogDbContext>(option =>
        option.UseInMemoryDatabase("CatalogDb"));


builder.Services.AddAutoMapper(typeof(ProductProfile));

builder.Services.AddGrpc(config =>
{
    config.EnableDetailedErrors = true;
});


var app = builder.Build();
//SeedDb(app);



app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");



static void SeedDb(WebApplication app)
{   
    var catalogContext = app.Services.GetRequiredService<CatalogDbContext>();
    CatalogDbContextSeed.SeedAsync(catalogContext);
}

app.Run();
