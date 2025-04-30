using MnemosyneDomain.Authorization;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Repositories;
using MnemosyneDomain.Test.Utilities;
using Moq;

namespace MnemosyneDomain.Test.Commands.NotebookPages
{
    public class CreateNotebookPageHandler
    {

        [SetUp]
        public void Setup()
        {
            var authHandlerMock = new Mock<IAuthorizationHandler>();

            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<It.IsAnyType>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(true);
            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<Guid>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(true);
            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(true);

            var notebookPages = new List<Models.NotebookPage>();

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Models.NotebookPage>()))
                .Returns((Models.NotebookPage page) => Task.CompletedTask);

            NotebookPageCommandHandler notebookPageCommandHandler = new(
                repositoryMock.Object,
                authHandlerMock.Object
            );    
        }

        [Test]
        public async Task ShouldReturnAValidNotebookPageCreatedEvent()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();

            var notebookPages = new List<Models.NotebookPage>();

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Models.NotebookPage>()))
                .Returns((Models.NotebookPage page) => Task.CompletedTask);

            NotebookPageCommandHandler notebookPageCommandHandler = new(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new CreateNotebookPage(new User(Guid.NewGuid()), 1);

            var result = await notebookPageCommandHandler.HandleAsync(command);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.NotebookId, Is.EqualTo(command.NotebookId));
            Assert.That(result.User.UserId, Is.EqualTo(command.User.UserId));
            Assert.That(result.NotebookPageId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.PageNumber, Is.EqualTo(0)); // First page should be 0

        }

        [Test]
        public async Task ShouldAddNotebookPageToRepository()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysAuthorizedMock();
            var notebookPages = new List<Models.NotebookPage>();

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Models.NotebookPage>()))
                .Returns((Models.NotebookPage page) => Task.CompletedTask);

            NotebookPageCommandHandler notebookPageCommandHandler = new(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new CreateNotebookPage(new User(Guid.NewGuid()), 1);

            var result = await notebookPageCommandHandler.HandleAsync(command);

            Assert.That(result, Is.Not.Null);
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Models.NotebookPage>()), Times.Once());
        }

        [Test]
        public async Task ShouldDoNothingIfUserIsNotAuthorized()
        {
            var authHandlerMock = MockAuthorizationHandler.GetAlwaysUnauthorizedMock();

            var notebookPages = new List<Models.NotebookPage>();

            var repositoryMock = new Mock<IRepository<Models.NotebookPage>>();

            repositoryMock.Setup(r => r.AddAsync(It.IsAny<Models.NotebookPage>()))
                .Returns((Models.NotebookPage page) => Task.CompletedTask);

            NotebookPageCommandHandler notebookPageCommandHandler = new(
                repositoryMock.Object,
                authHandlerMock.Object
            );

            var command = new CreateNotebookPage(new User(Guid.NewGuid()), 1);

            var result = await notebookPageCommandHandler.HandleAsync(command);

            Assert.That(result, Is.Null);
            repositoryMock.Verify(r => r.AddAsync(It.IsAny<Models.NotebookPage>()), Times.Never);
        }


    }
}
