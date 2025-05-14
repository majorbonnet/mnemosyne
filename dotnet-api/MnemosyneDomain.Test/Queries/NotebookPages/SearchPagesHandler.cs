using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Queries.Pages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Queries.Pages
{
    public class SearchPagesHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public SearchPagesHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldRetrievePagesThatMatchTheQueryForInexactMatches()
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
                Updated = DateTime.UtcNow,
                Title = "Notebook Title"
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

            context.Pages.Add(new Models.Page
            {
                NotebookId = notebook.NotebookId,
                PageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Another Title",
                Contents = "This is more content"
            });

            await context.SaveChangesAsync();

            // Act
            var queryHandler = new PageQueryHandler(context, authService);
            var result = await queryHandler.HandleAsync(new SearchPages(new MnemosyneDomain.Authorization.User(userId), "paging"));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task ShouldRetrievePagesThatMatchTheQueryForExactMatches()
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
                Updated = DateTime.UtcNow,
                Title = "Notebook Title"
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

            context.Pages.Add(new Models.Page
            {
                NotebookId = notebook.NotebookId,
                PageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Another Title",
                Contents = "This is more paging content"
            });

            await context.SaveChangesAsync();

            // Act
            var queryHandler = new PageQueryHandler(context, authService);
            var result = await queryHandler.HandleAsync(new SearchPages(new MnemosyneDomain.Authorization.User(userId), "\"paging\""));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task ShouldReturnEmptyIfQueryIsWhiteSpace()
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
                Updated = DateTime.UtcNow,
                Title = "Notebook Title"
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

            context.Pages.Add(new Models.Page
            {
                NotebookId = notebook.NotebookId,
                PageId = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Another Title",
                Contents = "This is more paging content"
            });

            await context.SaveChangesAsync();

            // Act
            var queryHandler = new PageQueryHandler(context, authService);
            var result = await queryHandler.HandleAsync(new SearchPages(new MnemosyneDomain.Authorization.User(userId), "     "));

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
