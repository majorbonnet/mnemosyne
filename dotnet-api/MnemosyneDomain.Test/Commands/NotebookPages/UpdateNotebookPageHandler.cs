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
    public class UpdateNotebookPageHandler
    {
        private const string DefaultTitle = "This is a title";
        private const string DefaultContents = "This is some contents";

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
                Title = DefaultTitle,
                Contents = DefaultContents
            });

            await _notebookPageRepository.AddPageAsync(new Models.NotebookPage
            {
                NotebookPageId = Guid.NewGuid(),
                NotebookId = 1,
                PageNumber = 1,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = DefaultTitle,
                Contents = DefaultContents
            });
        }

        [Test]
        public async Task ShouldUpdateNotebookPage()
        {
            _authorizationHandler.SetIsAuthorized(true);
            var command = new UpdateNotebookPage(new User(Guid.NewGuid()), 1, _notebookPageRepository.NotebookPages.First().NotebookPageId, "Updated Title", "Updated Contents");

            await _notebookPageCommandHandler.HandleAsync(command);

            var updatedPage = _notebookPageRepository.NotebookPages.First();
            Assert.That(updatedPage.Title, Is.EqualTo(command.Title));
            Assert.That(updatedPage.Contents, Is.EqualTo(command.Contents));
        }

        [Test]
        public async Task ShouldDoNothingIfUserIsNotAuthorized()
        {
            _authorizationHandler.SetIsAuthorized(false);
            var command = new UpdateNotebookPage(new User(Guid.NewGuid()), 1, _notebookPageRepository.NotebookPages.First().NotebookPageId, "Updated Title", "Updated Contents");

            await _notebookPageCommandHandler.HandleAsync(command);

            var updatedPage = _notebookPageRepository.NotebookPages.First();
            Assert.That(updatedPage.Title, Is.EqualTo(DefaultTitle));
            Assert.That(updatedPage.Contents, Is.EqualTo(DefaultContents));
        }
    }
}
