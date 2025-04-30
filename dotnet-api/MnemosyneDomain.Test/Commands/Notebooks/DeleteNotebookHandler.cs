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
    public class DeleteNotebookHandler
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

            // Pre-populate the repository with a notebook for testing
            _notebookRepository.Notebooks.Add(new Models.Notebook
            {
                NotebookId = 1,
                UserId = Guid.NewGuid(),
                Title = "Test Notebook",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            });
        }

        [Test]
        public async Task ShouldRemoveANotebookFromTheRepository()
        {
            // pre-condition: one notebook in the repository
            Assert.That(_notebookRepository.Notebooks.Count, Is.EqualTo(1));

            _authorizationHandler.SetIsAuthorized(true);
            var command = new DeleteNotebook(new User(Guid.NewGuid()), 1);

            await _handler.HandleAsync(command);

            Assert.That(_notebookRepository.Notebooks.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldDoNothingIfUserIsNotAuthorized()
        {
            // pre-condition: one notebook in the repository
            Assert.That(_notebookRepository.Notebooks.Count, Is.EqualTo(1));

            _authorizationHandler.SetIsAuthorized(false);
            var command = new DeleteNotebook(new User(Guid.NewGuid()), 1);

            await _handler.HandleAsync(command);

            Assert.That(_notebookRepository.Notebooks.Count, Is.EqualTo(1));
        }
    }
}
