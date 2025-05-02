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
        public async Task ShouldAuthorizeValidUserAndResource_Int()
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
            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);

            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(user, notebook.NotebookId, AuthorizationPolicies.NotebookOwner);

            // Assert
            Assert.True(isAuthorized);
        }

        [Fact]
        public async Task ShouldNotAuthorizeInvalidUser_Int()
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
            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);

            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(new User(Guid.NewGuid()), notebook.NotebookId, AuthorizationPolicies.NotebookOwner);

            // Assert
            Assert.False(isAuthorized);
        }

        [Fact]
        public async Task ShouldNotAuthorizeNonExistentResource_Int()
        {
            await _fixture.ResetDb();
            var context = _fixture.CreateContext();

            // Arrange
            Guid userId = Guid.NewGuid();
            var user = new User(userId);
            context.UserInfos.Add(new UserInfo { UserId = userId });
            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);

            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(user, -99, AuthorizationPolicies.NotebookOwner);

            // Assert
            Assert.False(isAuthorized);
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

            var notebookPage = new NotebookPage
            {
                Notebook = notebook,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PageNumber = 1,
                Title = "Test Page",
                Contents = "Test Content"
            };

            context.NotebookPages.Add(notebookPage);
            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);


            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(user, notebookPage.NotebookPageId, AuthorizationPolicies.NotebookPageOwner);

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

            var notebookPage = new NotebookPage
            {
                Notebook = notebook,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PageNumber = 1,
                Title = "Test Page",
                Contents = "Test Content"
            };

            context.NotebookPages.Add(notebookPage);
            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);

            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(new User(Guid.NewGuid()), notebookPage.NotebookPageId, AuthorizationPolicies.NotebookPageOwner);

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

            var notebookPage = new NotebookPage
            {
                Notebook = notebook,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                PageNumber = 1,
                Title = "Test Page",
                Contents = "Test Content"
            };

            context.NotebookPages.Add(notebookPage);

            await context.SaveChangesAsync();

            var handler = new MnemosyneDomain.Authorization.AuthorizationHandler(context);

            // Act
            var isAuthorized = await handler.IsAuthorizedAsync(user, Guid.NewGuid(), AuthorizationPolicies.NotebookPageOwner);

            // Assert
            Assert.False(isAuthorized);
        }
    }
}
