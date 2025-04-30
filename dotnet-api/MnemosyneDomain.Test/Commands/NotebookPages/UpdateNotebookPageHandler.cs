using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Repositories;
using MnemosyneDomain.Test.Utilities;
using Moq;

namespace MnemosyneDomain.Test.Commands.NotebookPages
{
    public class UpdateNotebookPageHandler
    {
        [Test]
        public async Task ShouldUpdateNotebookPage()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();

            repositoryMock.Setup(h => h.FindOneAsync(It.IsAny<Expression<Func<Models.NotebookPage, bool>>>()))
                .ReturnsAsync(new Models.NotebookPage
                {
                    NotebookPageId = Guid.NewGuid(),
                    NotebookId = 1,
                    PageNumber = 1,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Original Title",
                    Contents = "Original Contents"
                });

            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Models.NotebookPage>())).Verifiable();

            var notebookPageCommandHandler = new NotebookPageCommandHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var pageId = Guid.NewGuid();
            var command = new UpdateNotebookPage(new User(Guid.NewGuid()), 1, pageId, "Updated Title", "Updated Contents");

            await notebookPageCommandHandler.HandleAsync(command);

            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Models.NotebookPage>()), Times.Once);
        }

        [Test]
        public async Task ShouldDoNothingIfUserIsNotAuthorized()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysUnauthorizedMock();

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();

            repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Models.NotebookPage>())).Verifiable();

            var notebookPageCommandHandler = new NotebookPageCommandHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var pageId = Guid.NewGuid();
            var command = new UpdateNotebookPage(new User(Guid.NewGuid()), 1, pageId, "Updated Title", "Updated Contents");

            await notebookPageCommandHandler.HandleAsync(command);

            repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Models.NotebookPage>()), Times.Never);
        }
    }
}
