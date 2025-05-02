using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Pages;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Queries.Pages;
using MnemosyneDomain.Queries.Notebooks;

namespace MnemosyneDomain
{
    public static class IHostApplicationBuilderExtensions
    {
        public static void AddMnemosyneDomainServices(this IHostApplicationBuilder appBuilder)
        {
            appBuilder.Services.AddDbContext<MnemosyneContext>(opts =>
            {
                opts.UseNpgsql(appBuilder.Configuration.GetConnectionString("DefaultConnection"));
            });

            appBuilder.Services.AddScoped<IAuthorizationHandler, AuthorizationHandler>();
        }
    }
}
