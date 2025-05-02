using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Queries.Notebooks;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Queries.Notebooks
{
    public class GetNotebooksHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public GetNotebooksHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldRetrieveNotebooks()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();

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
            var queryHandler = new NotebookQueryHandler(context, FakeAuthorizationHandler.CreateAuthorized());
            var result = await queryHandler.HandleAsync(new GetNotebooks(new MnemosyneDomain.Authorization.User(userId)));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task ShouldRetrieveOnlyTheUsersNotebooks()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();

            Guid userId = Guid.NewGuid();
            Guid userId2 = Guid.NewGuid();
            context.UserInfos.Add(new Models.UserInfo { UserId = userId });
            context.UserInfos.Add(new Models.UserInfo { UserId = userId2 });

            var notebook = new Models.Notebook
            {
                NotebookId = Guid.NewGuid(),
                UserId = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            var notebook2 = new Models.Notebook
            {
                NotebookId = Guid.NewGuid(),
                UserId = userId2,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };


            context.Notebooks.Add(notebook);
            context.Notebooks.Add(notebook2);
            await context.SaveChangesAsync();

            // Act
            var queryHandler = new NotebookQueryHandler(context, FakeAuthorizationHandler.CreateAuthorized());
            var result = await queryHandler.HandleAsync(new GetNotebooks(new MnemosyneDomain.Authorization.User(userId)));

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task ShouldReturnEmptyIfNoNotebooksExistForUser()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();

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
            var queryHandler = new NotebookQueryHandler(context, FakeAuthorizationHandler.CreateAuthorized());

            // Simulate a different user
            var result = await queryHandler.HandleAsync(new GetNotebooks(new MnemosyneDomain.Authorization.User(Guid.NewGuid())));

            // Assert
            Assert.Empty(result);
        }
    }
}
