using IdentityServer4Auth;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
            .AddInMemoryClients(Config.Clients)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddDeveloperSigningCredential();

var app = builder.Build();


app.UseRouting();

app.UseIdentityServer();


app.MapGet("/", () => "Authentication!");

app.Run();
