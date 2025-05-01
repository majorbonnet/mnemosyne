using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Queries.NotebookPages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Queries.NotebookPages
{
    public class GetNotebookPagesHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public GetNotebookPagesHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldRetrieveNotebookPages()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();
            var authService = FakeAuthorizationHandler.CreateAuthorized();

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
                Title = "Page Title",
                Contents = "Page Contents"
            };

            context.NotebookPages.Add(notebookPage);
            await context.SaveChangesAsync();

            // Act
            var queryHandler = new NotebookPagesQueryHandler(context, authService);
            var result = await queryHandler.HandleAsync(new GetNotebookPages(new Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("Page Title", result.First().Title);
        }

        [Fact]
        public async Task ShouldReturnEmptyIfUnauthorized()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();
            var authService = FakeAuthorizationHandler.CreateUnauthorized();

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
                Title = "Page Title",
                Contents = "Page Contents"
            };

            context.NotebookPages.Add(notebookPage);
            await context.SaveChangesAsync();

            // Act
            var queryHandler = new NotebookPagesQueryHandler(context, authService);
            var result = await queryHandler.HandleAsync(new GetNotebookPages(new Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.Empty(result);
        }
    }
}
