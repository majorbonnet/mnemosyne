using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MnemosyneApi.Middleware;
using MnemosyneDomain;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Queries.Notebooks;

var builder = WebApplication.CreateBuilder(args);

builder.AddMnemosyneDomainServices();



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
app.UseEnsureUserInfo();

app.MapGet("/api/notebooks", [Authorize]([FromServices]NotebookQueryHandler notebookQueryHandler, ClaimsPrincipal user) =>
{
    string? userIdString = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

    if (userIdString is not null && Guid.TryParse(userIdString, out Guid userId))
    {
        var notebooks = notebookQueryHandler.GetNotebooksByUserId(new ByUserIdRequest(userId));
        return Results.Ok(notebooks);
    }

    return Results.Ok(new List<MnemosyneDomain.Queries.Notebooks.NotebookDto>());
});

app.MapPost("/api/notebooks", [Authorize] async ([FromServices] NotebookCommandHandler notebookCommandHandler, ClaimsPrincipal user) =>
{
    string? userIdString = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    if (userIdString is not null && Guid.TryParse(userIdString, out Guid userId))
    {
        MnemosyneDomain.Commands.Notebooks.NotebookDto notebook = await notebookCommandHandler.Handle(new AddNotebookRequest(userId));
        return Results.Ok(notebook);
    }
    return Results.BadRequest("Invalid user ID.");
});

app.MapGet("/", () => "");

app.Run();
