using DiscountGrpc.Protos;
using Microsoft.EntityFrameworkCore;
using ShoppingCartGrpc.Data;
using ShoppingCartGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (o => o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcConfigs:DiscountUrl")));



builder.Services.AddDbContext<ShoppingCartContext>(op =>
{
    op.UseInMemoryDatabase("ShoppingDb");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ShoppingCartService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
