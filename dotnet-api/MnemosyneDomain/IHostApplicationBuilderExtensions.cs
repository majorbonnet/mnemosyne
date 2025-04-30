using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Queries.NotebookPages;
using MnemosyneDomain.Queries.Notebooks;
using MnemosyneDomain.Repositories;

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

            appBuilder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            appBuilder.Services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            appBuilder.Services.AddScoped<AuthorizationHandler>();
        }
    }
}
