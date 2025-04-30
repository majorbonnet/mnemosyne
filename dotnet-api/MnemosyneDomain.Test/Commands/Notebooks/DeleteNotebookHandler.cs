using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Repositories;
using MnemosyneDomain.Test.Utilities;
using Moq;

namespace MnemosyneDomain.Test.Commands.Notebooks
{
    public class DeleteNotebookHandler
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task ShouldRemoveANotebookFromTheRepository()
        {
            var repositoryMock = new Mock<IRepository<Models.Notebook>>();
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Models.Notebook>()))
                .Returns(Task.CompletedTask);

            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();

            var handler = new NotebookCommandHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new DeleteNotebook(new User(Guid.NewGuid()), 1);

            await handler.HandleAsync(command);

            repositoryMock.Verify(r => r.DeleteAsync(It.Is<Models.Notebook>(n => n.NotebookId == 1)), Times.Once);
        }

        [Test]
        public async Task ShouldDoNothingIfUserIsNotAuthorized()
        {
            var repositoryMock = new Mock<IRepository<Models.Notebook>>();
            repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Models.Notebook>()))
                .Returns(Task.CompletedTask);

            var authHandlerMock = MockAuthorizationHandler.GetAlwaysUnauthorizedMock();

            var handler = new NotebookCommandHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new DeleteNotebook(new User(Guid.NewGuid()), 1);

            await handler.HandleAsync(command);

            repositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Models.Notebook>()), Times.Never);
        }
    }
}
