using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
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
    public class CreateNotebookHandler
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ShouldReturnAValidNotebookCreatedEvent()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();
            var repositoryMock = new Mock<IRepository<Models.Notebook>>();

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Models.Notebook>()))
                .Callback<Models.Notebook>(notebook =>
                {
                    notebook.NotebookId = new Random().Next(1, 1000); // Simulate ID generation
                });

            var handler = new NotebookCommandHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new CreateNotebook(new User(Guid.NewGuid()));

            var result = await handler.HandleAsync(command);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.User.UserId, Is.EqualTo(command.User.UserId));
            Assert.That(result.NotebookId, Is.Not.EqualTo(0));
        }

        [Test]
        public async Task ShouldAddANotebookToTheRepository()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();
            var repositoryMock = new Mock<IRepository<Models.Notebook>>();

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Models.Notebook>()))
                .Callback<Models.Notebook>(notebook =>
                {
                    notebook.NotebookId = new Random().Next(1, 1000); // Simulate ID generation
                });

            var handler = new NotebookCommandHandler(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new CreateNotebook(new User(Guid.NewGuid()));

            var result = await handler.HandleAsync(command);
            Assert.That(result, Is.Not.Null);
            repositoryMock.Verify(r => r.AddAsync(It.Is<Models.Notebook>(n =>
                n.UserId == command.User.UserId &&
                n.NotebookId == result.NotebookId
            )), Times.Once);
        }
    }
}
