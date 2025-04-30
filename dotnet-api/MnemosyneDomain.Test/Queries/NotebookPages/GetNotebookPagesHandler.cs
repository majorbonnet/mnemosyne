using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Queries.NotebookPages;
using MnemosyneDomain.Repositories;
using MnemosyneDomain.Test.Utilities;
using Moq;

namespace MnemosyneDomain.Test.Queries.NotebookPages
{
    public class GetNotebookPagesHandler
    {
        [Test]
        public async Task ShouldReturnThePagesForANotebook()
        {
             var pages = new List<Models.NotebookPage> {
                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 1,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 1,
                    PageNumber = 1,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 2,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 3,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 3,
                    PageNumber = 1,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                } };

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();
            repositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Models.NotebookPage, bool>>>()))
                .Returns((Expression<Func<Models.NotebookPage, bool>> predicate) =>
                    pages.Where(predicate.Compile()).AsQueryable());

            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();

            var notebookPageQueryHandler = new NotebookPagesQueryHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var query = new GetNotebookPages(new User(Guid.NewGuid()), 1);

            var result = await notebookPageQueryHandler.HandleAsync(query);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldReturnAnEmptyListIfUserIsNotAuthorized()
        {
            var pages = new List<Models.NotebookPage> {
                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 1,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 1,
                    PageNumber = 1,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 2,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 3,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 3,
                    PageNumber = 1,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                } };

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();
            repositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Models.NotebookPage, bool>>>()))
                .Returns((Func<Models.NotebookPage, bool> predicate) =>
                    pages.Where(predicate).AsQueryable());

            var authHandlerMock = MockAuthorizationHandler.GetAlwaysUnauthorizedMock();

            var notebookPageQueryHandler = new NotebookPagesQueryHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var query = new GetNotebookPages(new User(Guid.NewGuid()), 1);

            var result = await notebookPageQueryHandler.HandleAsync(query);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnAnEmptyListIfNotebookIdHasNoPages()
        {
            var pages = new List<Models.NotebookPage> {
                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 1,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 1,
                    PageNumber = 1,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 2,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 3,
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                },

                new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 3,
                    PageNumber = 1,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page",
                    Contents = "This is a test page."
                } };

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();
            repositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Models.NotebookPage, bool>>>()))
                .Returns((Expression<Func<Models.NotebookPage, bool>> predicate) =>
                    pages.Where(predicate.Compile()).AsQueryable());

            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();

            var notebookPageQueryHandler = new NotebookPagesQueryHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var query = new GetNotebookPages(new User(Guid.NewGuid()), 5);

            var result = await notebookPageQueryHandler.HandleAsync(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
