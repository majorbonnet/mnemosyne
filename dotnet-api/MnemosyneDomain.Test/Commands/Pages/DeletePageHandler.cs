using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Commands.Pages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Commands.Pages
{
    public class DeletePageHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public DeletePageHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldDeleteAPage()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();
            var authService = FakeAuthorizationHandler.CreateAuthorized();
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

            var page = new Models.Page
            {
                NotebookId = notebook.NotebookId,
                PageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            Assert.Equal(1, context.Pages.Count());

            // Act
            await commandHandler.HandleAsync(new DeletePage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId, page.PageId));

            // Assert
            Assert.Equal(0, context.Pages.Count());
        }

        [Fact]
        public async Task ShouldDoNothingIfUserIsUnauthorized()
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

            var page = new Models.Page
            {
                NotebookId = notebook.NotebookId,
                PageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            Assert.Equal(1, context.Pages.Count());

            // Act
            await commandHandler.HandleAsync(new DeletePage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId, page.PageId));

            // Assert
            Assert.Equal(1, context.Pages.Count());
        }
    }
}
