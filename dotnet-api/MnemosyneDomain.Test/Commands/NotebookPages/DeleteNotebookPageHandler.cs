using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Commands.NotebookPages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Commands.NotebookPages
{
    public class DeleteNotebookPageHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public DeleteNotebookPageHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldDeleteANotebookPage()
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
                Updated = DateTime.UtcNow
            };

            context.NotebookPages.Add(notebookPage);
            await context.SaveChangesAsync();

            Assert.Equal(1, context.NotebookPages.Count());

            // Act
            await commandHandler.HandleAsync(new DeleteNotebookPage(new Authorization.User(userId), notebook.NotebookId, notebookPage.NotebookPageId));

            // Assert
            Assert.Equal(0, context.NotebookPages.Count());
        }

        [Fact]
        public async Task ShouldDoNothingIfUserIsUnauthorized()
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
                Updated = DateTime.UtcNow
            };

            context.NotebookPages.Add(notebookPage);
            await context.SaveChangesAsync();

            Assert.Equal(1, context.NotebookPages.Count());

            // Act
            await commandHandler.HandleAsync(new DeleteNotebookPage(new Authorization.User(userId), notebook.NotebookId, notebookPage.NotebookPageId));

            // Assert
            Assert.Equal(1, context.NotebookPages.Count());
        }
    }
}
