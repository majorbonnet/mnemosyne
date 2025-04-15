using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MnemosyneDomain;
using MnemosyneDomain.Queries.Journals;

var builder = WebApplication.CreateBuilder(args);

builder.AddMnemosyneDomainServices();



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, async o =>
    {
        // Set the metadata address for the OpenID configuration
        o.MetadataAddress = "https://localhost:8443/realms/mnemosyne/.well-known/openid-configuration";

        // Set the authority for the authentication server
        o.Authority = "https://localhost:8443/realms/mnemosyne";

        // Set the audience for the JWT token
        o.Audience = "account";

        // TODO: fix the SSL issues or turn off SSL for dev
        // currently getting a root cert untrusted error getting the JWKS from Keycloak in development
        var handler = new HttpClientHandler()
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        HttpClient client = new HttpClient(handler);
        var jwksJson  = await (await client.GetAsync("https://localhost:8443/realms/mnemosyne/protocol/openid-connect/certs")).Content.ReadAsStringAsync();
        var jwks = new JsonWebKeySet(jwksJson);
        var jwk = jwks.GetSigningKeys().FirstOrDefault();

        o.TokenValidationParameters.IssuerSigningKey = jwk;
        o.TokenValidationParameters.ValidIssuer = "https://localhost:8443/realms/mnemosyne";
        o.TokenValidationParameters.ValidAudience = "account";

        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = (context) =>
            {
                var exception = context?.Exception;
                return Task.CompletedTask;
            }
        };
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

app.MapGet("/api/journals", [Authorize]([FromServices]JournalQueryHandler journalQueryHandler, ClaimsPrincipal user) =>
{
    string? userIdString = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    if (userIdString is not null && Guid.TryParse(userIdString, out Guid userId))
    {
        var journals = journalQueryHandler.GetJournalsByUserId(new ByUserIdRequest(userId));
        return Results.Ok(journals);
    }

    return Results.Ok(new List<JournalDto>());
});

app.MapGet("/", () => "");

app.Run();
