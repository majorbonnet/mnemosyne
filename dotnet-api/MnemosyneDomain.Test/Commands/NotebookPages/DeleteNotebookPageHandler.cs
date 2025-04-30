using System;
using System.Collections.Generic;
using System.Linq;
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
    public class DeleteNotebookPageHandler
    {

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task ShouldRemoveNotebookPageFromRepository()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Models.NotebookPage>()));

            var notebookPageCommandHandler = new NotebookPageCommandHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new DeleteNotebookPage(new User(Guid.NewGuid()), 1, Guid.NewGuid());

            await notebookPageCommandHandler.HandleAsync(command);

            repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Models.NotebookPage>()), Times.Once);
        }

        [Test]
        public async Task ShouldDoNothingIfUserIsNotAuthorized()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysUnauthorizedMock();

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Models.NotebookPage>()));

            var notebookPageCommandHandler = new NotebookPageCommandHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new DeleteNotebookPage(new User(Guid.NewGuid()), 1, Guid.NewGuid());

            await notebookPageCommandHandler.HandleAsync(command);

            repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Models.NotebookPage>()), Times.Never);
        }
    }
}
