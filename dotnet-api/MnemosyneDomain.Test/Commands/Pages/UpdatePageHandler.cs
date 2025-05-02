using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Commands.Pages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Commands.Pages
{
    public class UpdatePageHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public UpdatePageHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldUpdateAPage()
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
                Updated = DateTime.UtcNow,
                Contents = "Old Contents"
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            // Act
            await commandHandler.HandleAsync(new UpdatePage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId, page.PageId, "New Contents"));

            // Assert
            context.Entry(page).Reload();
            Assert.Equal("New Contents", page.Contents);
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

            var page = new Models.Page
            {
                NotebookId = notebook.NotebookId,
                PageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Old Title",
                Contents = "Old Contents"
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            // Act
            await commandHandler.HandleAsync(new UpdatePage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId, page.PageId, "New contents"));

            // Assert
            var updatedPage = await context.Pages.FindAsync(page.PageId);

            Assert.NotNull(updatedPage);
            Assert.Equal("Old Contents", updatedPage.Contents);
        }
    }
}
