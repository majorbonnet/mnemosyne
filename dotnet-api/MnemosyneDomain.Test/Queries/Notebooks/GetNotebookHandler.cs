using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Queries.Notebooks;

namespace MnemosyneDomain.Test.Queries.Notebooks
{
    public class GetNotebookHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public GetNotebookHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldRetrieveANotebook()
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
            var result = await queryHandler.HandleAsync(new GetNotebook(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(notebook.NotebookId, result.NotebookId);
        }

        [Fact]
        public async Task ShouldReturnNullIfNotebookDoesntExist()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();

            Guid userId = Guid.NewGuid();
            context.UserInfos.Add(new Models.UserInfo { UserId = userId });

            var notebook = new Models.Notebook
            {
                NotebookId = Guid.NewGuid(),
                UserId = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Notebooks.Add(notebook);
            await context.SaveChangesAsync();

            // Act
            var queryHandler = new NotebookQueryHandler(context, FakeAuthorizationHandler.CreateAuthorized());
            var result = await queryHandler.HandleAsync(new GetNotebook(new MnemosyneDomain.Authorization.User(userId), Guid.NewGuid()));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task ShouldReturnNullIfUserIsUnauthorized()
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
            var queryHandler = new NotebookQueryHandler(context, FakeAuthorizationHandler.CreateUnauthorized());
            var result = await queryHandler.HandleAsync(new GetNotebook(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.Null(result);
        }
    }
}
