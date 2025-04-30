using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Repositories;
using MnemosyneDomain.Test.Fakes;

namespace MnemosyneDomain.Test.Commands.NotebookPages
{
    public class DeleteNotebookPageHandler
    {
        private FakeAuthorizationHandler _authorizationHandler;
        private FakeNotebookPageRepository _notebookPageRepository;
        private NotebookPageCommandHandler _notebookPageCommandHandler;

        [SetUp]
        public async Task Setup()
        {
            _authorizationHandler = new FakeAuthorizationHandler();
            _notebookPageRepository = new FakeNotebookPageRepository();
            _notebookPageCommandHandler = new NotebookPageCommandHandler(
                _notebookPageRepository,
                _authorizationHandler
            );

            await _notebookPageRepository.AddPageAsync(new Models.NotebookPage
            {
                NotebookPageId = Guid.NewGuid(),
                NotebookId = 1,
                PageNumber = 0,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Test Page",
                Contents = "This is a test page."
            });

            await _notebookPageRepository.AddPageAsync(new Models.NotebookPage
            {
                NotebookPageId = Guid.NewGuid(),
                NotebookId = 1,
                PageNumber = 1,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Test Page",
                Contents = "This is a test page."
            });
        }

        [Test]
        public async Task ShouldRemoveNotebookPageFromRepository()
        {
            // pre-condition: select a page and assert that it exists in the repository
            Guid pageId = _notebookPageRepository.NotebookPages.First().NotebookPageId;
            Assert.That(await _notebookPageRepository.GetPageByIdAsync(pageId), Is.Not.Null);

            _authorizationHandler.SetIsAuthorized(true);
            var command = new DeleteNotebookPage(new User(Guid.NewGuid()), 1, pageId);

            await _notebookPageCommandHandler.HandleAsync(command);

            Assert.That(await _notebookPageRepository.GetPageByIdAsync(command.NotebookPageId), Is.Null);
        }

        [Test]
        public async Task ShouldDoNothingIfUserIsNotAuthorized()
        {
            // pre-condition: select a page and assert that it exists in the repository
            Guid pageId = _notebookPageRepository.NotebookPages.First().NotebookPageId;
            Assert.That(await _notebookPageRepository.GetPageByIdAsync(pageId), Is.Not.Null);

            _authorizationHandler.SetIsAuthorized(false);
            var command = new DeleteNotebookPage(new User(Guid.NewGuid()), 1, pageId);

            await _notebookPageCommandHandler.HandleAsync(command);

            Assert.That(await _notebookPageRepository.GetPageByIdAsync(command.NotebookPageId), Is.Not.Null);
        }
    }
}
