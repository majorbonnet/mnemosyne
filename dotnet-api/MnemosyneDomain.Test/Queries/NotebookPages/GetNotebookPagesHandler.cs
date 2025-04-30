using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Commands.NotebookPages;
using MnemosyneDomain.Queries.NotebookPages;
using MnemosyneDomain.Repositories;
using MnemosyneDomain.Test.Fakes;

namespace MnemosyneDomain.Test.Queries.NotebookPages
{
    public class GetNotebookPagesHandler
    {
        private FakeAuthorizationHandler _authorizationHandler;
        private FakeNotebookPageRepository _notebookPageRepository;
        private NotebookPagesQueryHandler _notebookPageQueryHandler;

        [SetUp]
        public async Task Setup()
        {
            _authorizationHandler = new FakeAuthorizationHandler();
            _notebookPageRepository = new FakeNotebookPageRepository();
            _notebookPageQueryHandler = new NotebookPagesQueryHandler(
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

            await _notebookPageRepository.AddPageAsync(new Models.NotebookPage
            {
                NotebookPageId = Guid.NewGuid(),
                NotebookId = 2,
                PageNumber = 0,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Test Page",
                Contents = "This is a test page."
            });

            await _notebookPageRepository.AddPageAsync(new Models.NotebookPage
            {
                NotebookPageId = Guid.NewGuid(),
                NotebookId = 3,
                PageNumber = 0,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Test Page",
                Contents = "This is a test page."
            });

            await _notebookPageRepository.AddPageAsync(new Models.NotebookPage
            {
                NotebookPageId = Guid.NewGuid(),
                NotebookId = 3,
                PageNumber = 1,
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow,
                Title = "Test Page",
                Contents = "This is a test page."
            });
        }

        [Test]
        public async Task ShouldReturnThePagesForANotebook()
        {
            _authorizationHandler.SetIsAuthorized(true);

            // Assert the pre-condition is true
            Assert.That(_notebookPageRepository.NotebookPages.Count(p => p.NotebookId == 1), Is.EqualTo(2));

            var query = new GetNotebookPages(new User(Guid.NewGuid()), 1);

            var result = await _notebookPageQueryHandler.HandleAsync(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task ShouldReturnAnEmptyListIfUserIsNotAuthorized()
        {
            _authorizationHandler.SetIsAuthorized(false);

            // Assert the pre-condition is true
            Assert.That(_notebookPageRepository.NotebookPages.Count(p => p.NotebookId == 1), Is.EqualTo(2));

            var query = new GetNotebookPages(new User(Guid.NewGuid()), 1);

            var result = await _notebookPageQueryHandler.HandleAsync(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task ShouldReturnAnEmptyListIfNotebookIdHasNoPages()
        {
            _authorizationHandler.SetIsAuthorized(false);

            // Assert the pre-condition is true and no-one has added a page for NotebookId 5 to setup
            Assert.That(_notebookPageRepository.NotebookPages.Count(p => p.NotebookId == 5), Is.EqualTo(0));

            var query = new GetNotebookPages(new User(Guid.NewGuid()), 5);

            var result = await _notebookPageQueryHandler.HandleAsync(query);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}
