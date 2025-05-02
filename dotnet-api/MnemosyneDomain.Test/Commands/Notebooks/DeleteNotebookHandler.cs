using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Commands.Notebooks;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Commands.Notebooks
{
    public class DeleteNotebookHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;
        public DeleteNotebookHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldDeleteANotebook()
        {
            await _fixture.ResetDb();
            // Arrange
            // we need separate contexts for the inserts in the test and the command handler
            // to prevent EF from thinking we are attaching two entities with the same ID
            var context = _fixture.CreateContext();

            var authService = FakeAuthorizationHandler.CreateAuthorized();
            var commandHandler = new NotebookCommandHandler(_fixture.CreateContext(), authService);

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

            Assert.Equal(1, context.Notebooks.Count());

            // Act
            await commandHandler.HandleAsync(new DeleteNotebook(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.Equal(0, context.Notebooks.Count());
 
        }

        [Fact]
        public async Task ShouldDoNothingIfUserIsUnauthorized()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();

            var authService = FakeAuthorizationHandler.CreateUnauthorized();
            var commandHandler = new NotebookCommandHandler(_fixture.CreateContext(), authService);

            Guid userId = Guid.NewGuid();

            context.UserInfos.Add(new Models.UserInfo { UserId = userId });

            var notebook = new Models.Notebook
            {
                NotebookId = 0,
                UserId = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Notebooks.Add(notebook);
            await context.SaveChangesAsync();

            Assert.Equal(1, context.Notebooks.Count());

            // Act
            await commandHandler.HandleAsync(new DeleteNotebook(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.Equal(1, context.Notebooks.Count());
        }
    }
}
