using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Repositories;
using MnemosyneDomain.Test.Fakes;

namespace MnemosyneDomain.Test.Commands.NotebookPages
{
    public class CreateNotebookPageHandler
    {
        private FakeAuthorizationHandler _authorizationHandler;
        private FakeNotebookPageRepository _notebookPageRepository;
        private NotebookPageCommandHandler _notebookPageCommandHandler;

        [SetUp]
        public void Setup()
        {
            _authorizationHandler = new FakeAuthorizationHandler();
            _notebookPageRepository = new FakeNotebookPageRepository();
            _notebookPageCommandHandler = new NotebookPageCommandHandler(
                _notebookPageRepository,
                _authorizationHandler
            );    
        }

        [Test]
        public async Task ShouldReturnAValidNotebookPageCreatedEvent()
        {
            _authorizationHandler.SetIsAuthorized(true);
            var command = new CreateNotebookPage(new User(Guid.NewGuid()), 1);

            var result = await _notebookPageCommandHandler.HandleAsync(command);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.NotebookId, Is.EqualTo(command.NotebookId));
            Assert.That(result.User.UserId, Is.EqualTo(command.User.UserId));
            Assert.That(result.NotebookPageId, Is.Not.EqualTo(Guid.Empty));
            Assert.That(result.PageNumber, Is.EqualTo(0)); // First page should be 0

        }

        [Test]
        public async Task ShouldAddNotebookPageToRepository()
        {
            _authorizationHandler.SetIsAuthorized(true);
            var command = new CreateNotebookPage(new User(Guid.NewGuid()), 1);

            var result = await _notebookPageCommandHandler.HandleAsync(command);

            Assert.That(result, Is.Not.Null);
            Assert.That(_notebookPageRepository.NotebookPages.FirstOrDefault(p => p.NotebookPageId == result.NotebookPageId), Is.Not.Null);
        }

        [Test]
        public async Task ShouldDoNothingIfUserIsNotAuthorized()
        {
            _authorizationHandler.SetIsAuthorized(false);
            var command = new CreateNotebookPage(new User(Guid.NewGuid()), 1);

            var result = await _notebookPageCommandHandler.HandleAsync(command);

            Assert.That(result, Is.Null);
            Assert.That((await _notebookPageRepository.GetPagesByNotebookIdAsync(1)).Count(), Is.EqualTo(0));
        }


    }
}
