using System.Net.WebSockets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MnemosyneApi.Middleware;
using MnemosyneDomain;
using Wolverine;
using Wolverine.Http;

var builder = WebApplication.CreateBuilder(args);

builder.AddMnemosyneDomainServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseWolverine(opts =>
{
    opts.Discovery.IncludeAssembly(typeof(MnemosyneContext).Assembly);
});
builder.Services.AddWolverineHttp();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, /*async*/ o =>
    {
        o.RequireHttpsMetadata = false;
        // Set the metadata address for the OpenID configuration
        o.MetadataAddress = "http://localhost:8080/realms/mnemosyne/.well-known/openid-configuration";

        // Set the authority for the authentication server
        o.Authority = "http://localhost:8080/realms/mnemosyne";

        // Set the audience for the JWT token
        o.Audience = "account";
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "localorigins",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                      });
});

var app = builder.Build();

app.UseCors("localorigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

app.MapWolverineEndpoints(opts => 
{
    opts.RequireAuthorizeOnAll();
    opts.AddMiddleware(typeof(VerifyUserMiddleware));
});

app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
