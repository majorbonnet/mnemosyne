using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Models;
using Xunit;

namespace MnemosyneDomain.Test.Authorization
{
    public class AuthorizationHandler : IClassFixture<DatabaseContainerFixture>
    {
        private readonly DatabaseContainerFixture _fixture;

        public AuthorizationHandler(DatabaseContainerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ShouldAuthorizeValidUserAndResource_Guid()
        {
            await _fixture.ResetDb();
            var context = _fixture.CreateContext();

            // Arrange
            Guid userId = Guid.NewGuid();
            var user = new User(userId);
            context.UserInfos.Add(new UserInfo { UserId = userId });

            var notebook = new Notebook
            {
                UserId = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Notebooks.Add(notebook);

            var page = new Page
            {
                Notebook = notebook,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PageNumber = 1,
                Title = "Test Page",
                Contents = "Test Content"
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);


            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(user, page.PageId, AuthorizationPolicies.PageOwner);

            // Assert
            Assert.True(isAuthorized);
        }

        [Fact]
        public async Task ShouldNotAuthorizeInvalidUser_Guid()
        {
            await _fixture.ResetDb();
            var context = _fixture.CreateContext();

            // Arrange
            Guid userId = Guid.NewGuid();
            var user = new User(userId);
            context.UserInfos.Add(new UserInfo { UserId = userId });

            var notebook = new Notebook
            {
                UserId = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Notebooks.Add(notebook);

            var page = new Page
            {
                Notebook = notebook,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PageNumber = 1,
                Title = "Test Page",
                Contents = "Test Content"
            };

            context.Pages.Add(page);
            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);

            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(new User(Guid.NewGuid()), page.PageId, AuthorizationPolicies.PageOwner);

            // Assert
            Assert.False(isAuthorized);
        }

        [Fact]
        public async Task ShouldNotAuthorizeNonExistentResource_Guid()
        {
            await _fixture.ResetDb();
            var context = _fixture.CreateContext();

            // Arrange
            Guid userId = Guid.NewGuid();
            var user = new User(userId);
            context.UserInfos.Add(new UserInfo { UserId = userId });

            var notebook = new Notebook
            {
                UserId = userId,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            };

            context.Notebooks.Add(notebook);

            var page = new Page
            {
                Notebook = notebook,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PageNumber = 1,
                Title = "Test Page",
                Contents = "Test Content"
            };

            context.Pages.Add(page);

            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);

            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(user, Guid.NewGuid(), AuthorizationPolicies.PageOwner);

            // Assert
            Assert.False(isAuthorized);
        }
    }
}
