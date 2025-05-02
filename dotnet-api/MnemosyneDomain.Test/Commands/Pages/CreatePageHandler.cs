using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Commands.Pages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Commands.Pages
{
    public class CreatePageHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public CreatePageHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldCreateAPage()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();
            var authService = FakeAuthorizationHandler.CreateAuthorized();
            var commandHandler = new PageCommandHandler(context, authService);

            Guid userId = Guid.NewGuid();
            context.UserInfos.Add(new Models.UserInfo { UserId = userId });

            var notebook = new Models.Notebook
            {
                UserId = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Notebooks.Add(notebook);
            await context.SaveChangesAsync();

            // Act
            var result = await commandHandler.HandleAsync(new CreatePage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(notebook.NotebookId, result.NotebookId);
            Assert.NotEqual(Guid.Empty, result.PageId);
        }

        [Fact]
        public async Task ShouldDoNothingIfNotAuthorized()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();
            var authService = FakeAuthorizationHandler.CreateUnauthorized();
            var commandHandler = new PageCommandHandler(_fixture.CreateContext(), authService);

            Guid userId = Guid.NewGuid();
            context.UserInfos.Add(new Models.UserInfo { UserId = userId });

            var notebook = new Models.Notebook
            {
                UserId = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Notebooks.Add(notebook);
            await context.SaveChangesAsync();

            // Act
            var result = await commandHandler.HandleAsync(new CreatePage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.Null(result);
            Assert.Equal(0, context.Pages.Count());
        }
    }
}
