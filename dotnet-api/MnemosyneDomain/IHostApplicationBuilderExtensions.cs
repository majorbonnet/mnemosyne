using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MnemosyneDomain.Queries.Journals;

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

            appBuilder.Services.AddScoped<JournalQueryHandler>();
        }
    }
}
