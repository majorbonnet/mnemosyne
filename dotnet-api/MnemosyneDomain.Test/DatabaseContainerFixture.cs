using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test
{
    public class DatabaseContainerFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
            .WithImage("postgres:17.4")
            .WithDatabase("mnemosyne")
            .Build();

        public PostgreSqlContainer Container => _postgres;

        public async Task<MnemosyneContext> CreateContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<MnemosyneContext>()
                .UseNpgsql(_postgres.GetConnectionString())
                .Options;

            var context = new MnemosyneContext(dbContextOptions);

            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();

            return context;
        }

        public Task InitializeAsync()
        {
            return _postgres.StartAsync();
        }

        public Task DisposeAsync()
        {
            return _postgres.DisposeAsync().AsTask();
        }
    }
}
