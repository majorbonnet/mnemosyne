using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Commands.NotebookPages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Commands.NotebookPages
{
    public class UpdateNotebookPageHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public UpdateNotebookPageHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldUpdateANotebookPage()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();
            var authService = FakeAuthorizationHandler.CreateAuthorized();
            var commandHandler = new NotebookPageCommandHandler(_fixture.CreateContext(), authService);

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

            var notebookPage = new Models.NotebookPage
            {
                NotebookId = notebook.NotebookId,
                NotebookPageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Old Title",
                Contents = "Old Contents"
            };

            context.NotebookPages.Add(notebookPage);
            await context.SaveChangesAsync();

            // Act
            await commandHandler.HandleAsync(new UpdateNotebookPage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId, notebookPage.NotebookPageId, "New Title", "New Contents"));

            // Assert
            context.Entry(notebookPage).Reload();
            Assert.Equal("New Title", notebookPage.Title);
            Assert.Equal("New Contents", notebookPage.Contents);
        }

        [Fact]
        public async Task ShouldDoNothingIfNotAuthorized()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();
            var authService = FakeAuthorizationHandler.CreateUnauthorized();
            var commandHandler = new NotebookPageCommandHandler(_fixture.CreateContext(), authService);

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

            var notebookPage = new Models.NotebookPage
            {
                NotebookId = notebook.NotebookId,
                NotebookPageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Old Title",
                Contents = "Old Contents"
            };

            context.NotebookPages.Add(notebookPage);
            await context.SaveChangesAsync();

            // Act
            await commandHandler.HandleAsync(new UpdateNotebookPage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId, notebookPage.NotebookPageId, "New Title", "New Contents"));

            // Assert
            var updatedPage = await context.NotebookPages.FindAsync(notebookPage.NotebookPageId);

            Assert.NotNull(updatedPage);
            Assert.Equal("Old Title", updatedPage.Title);
            Assert.Equal("Old Contents", updatedPage.Contents);
        }
    }
}
