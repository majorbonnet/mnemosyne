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

    public class CreateNotebookHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public CreateNotebookHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldCreateANotebook()
        {
            // Arrange
            var context = await _fixture.CreateContext();

            var authService = FakeAuthorizationHandler.CreateAuthorized();
            var commandHandler = new NotebookCommandHandler(context, authService);

            Guid userId = Guid.NewGuid();

            // user needs to exist in db
            context.UserInfos.Add(new Models.UserInfo { UserId = userId });

            // Act
            var result = await commandHandler.HandleAsync(new CreateNotebook(new Authorization.User(userId)));

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.User.UserId);
            Assert.NotEqual(0, result.NotebookId);
            Assert.True(result.Created < DateTime.UtcNow);
            Assert.True(result.Updated < DateTime.UtcNow);
        }
    }
}
