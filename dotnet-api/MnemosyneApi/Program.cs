using System.Net.WebSockets;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using MnemosyneApi.Hubs;
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
    opts.Discovery.IncludeAssembly(typeof(Program).Assembly);
});
builder.Services.AddWolverineHttp();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
    {
        o.RequireHttpsMetadata = false;
        // Set the metadata address for the OpenID configuration
        o.MetadataAddress = "http://localhost:8080/realms/mnemosyne/.well-known/openid-configuration";

        // Set the authority for the authentication server
        o.Authority = "http://localhost:8080/realms/mnemosyne";

        // Set the audience for the JWT token
        o.Audience = "account";

        // Sending the access token in the query string is required when using WebSockets or ServerSentEvents
        // due to a limitation in Browser APIs. We restrict it to only calls to the
        // SignalR hub in this code.
        // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
        // for more information about security considerations when using
        // the query string to transmit the access token.
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs/notebooksync")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "localorigins",
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:5173")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
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

app.MapHub<NotebookSyncHub>("/hubs/notebooksync");


app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
