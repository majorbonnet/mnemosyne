using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Repositories;
using MnemosyneDomain.Test.Fakes;

namespace MnemosyneDomain.Test.Commands.Notebooks
{
    public class CreateNotebookHandler
    {
        private FakeNotebookRepository _notebookRepository;
        private FakeAuthorizationHandler _authorizationHandler;
        private NotebookCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _notebookRepository = new FakeNotebookRepository();
            _authorizationHandler = new FakeAuthorizationHandler();

            _handler = new NotebookCommandHandler(
                _notebookRepository,
                _authorizationHandler
            );
        }

        [Test]
        public async Task ShouldReturnAValidNotebookCreatedEvent()
        {
            _authorizationHandler.SetIsAuthorized(true);
            var command = new CreateNotebook(new User(Guid.NewGuid()));

            var result = await _handler.HandleAsync(command);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.User.UserId, Is.EqualTo(command.User.UserId));
            Assert.That(result.NotebookId, Is.Not.EqualTo(0));
        }

        [Test]
        public async Task ShouldAddANotebookToTheRepository()
        {
            // pre-condition: no notebooks in the repository
            Assert.That(_notebookRepository.Notebooks.Count, Is.EqualTo(0));

            _authorizationHandler.SetIsAuthorized(true);
            var command = new CreateNotebook(new User(Guid.NewGuid()));

            var result = await _handler.HandleAsync(command);
            Assert.That(result, Is.Not.Null);
            Assert.That( _notebookRepository.Notebooks.Count, Is.Not.EqualTo(0));
            Assert.That(_notebookRepository.Notebooks.First().NotebookId, Is.EqualTo(result.NotebookId));
        }
    }
}
