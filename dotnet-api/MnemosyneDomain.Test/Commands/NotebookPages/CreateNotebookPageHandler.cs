using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Commands.NotebookPages;
using Testcontainers.PostgreSql;

namespace MnemosyneDomain.Test.Commands.NotebookPages
{
    public class CreateNotebookPageHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public CreateNotebookPageHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldCreateANotebookPage()
        {
            await _fixture.ResetDb();
            // Arrange
            var context = _fixture.CreateContext();
            var authService = FakeAuthorizationHandler.CreateAuthorized();
            var commandHandler = new NotebookPageCommandHandler(context, authService);

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
            var result = await commandHandler.HandleAsync(new CreateNotebookPage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(notebook.NotebookId, result.NotebookId);
            Assert.NotEqual(Guid.Empty, result.NotebookPageId);
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

            // Act
            var result = await commandHandler.HandleAsync(new CreateNotebookPage(new MnemosyneDomain.Authorization.User(userId), notebook.NotebookId));

            // Assert
            Assert.Null(result);
            Assert.Equal(0, context.NotebookPages.Count());
        }
    }
}
