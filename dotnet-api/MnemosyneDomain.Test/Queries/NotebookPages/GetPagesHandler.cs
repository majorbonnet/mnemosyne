using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Queries.Pages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Queries.Pages
{
    public class GetPagesHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public GetPagesHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldRetrievePages()
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

            var page = new Models.Page
            {
                NotebookId = notebook.NotebookId,
                PageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Page Title",
                Contents = "Page Contents"
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            // Act
            var queryHandler = new PageQueryHandler(context, authService);
            var result = await queryHandler.HandleAsync(new GetPages(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

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

            var page = new Models.Page
            {
                NotebookId = notebook.NotebookId,
                PageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Page Title",
                Contents = "Page Contents"
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            // Act
            var queryHandler = new PageQueryHandler(context, authService);
            var result = await queryHandler.HandleAsync(new GetPages(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.Empty(result);
        }
    }
}
