using DiscountGrpc.Protos;
using Microsoft.EntityFrameworkCore;
using ShoppingCartGrpc.Data;
using ShoppingCartGrpc.Mapper;
using ShoppingCartGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DiscountService>();


builder.Services.AddDbContext<ShoppingCartContext>(op =>
{
    op.UseInMemoryDatabase("ShoppingDb");
});

builder.Services.AddGrpc(opt =>
{
    opt.EnableDetailedErrors = true;
});

builder.Services.AddAutoMapper(typeof(ShoppingCartProfile));

builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
    (o => o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcConfigs:DiscountUrl")));


builder.Services.AddAuthentication("bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5005";
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateAudience = false,
        };
    });

builder.Services.AddAuthorization();


var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<ShoppingCartService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
