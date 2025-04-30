using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Queries.Notebooks;
using MnemosyneDomain.Test.Fakes;

namespace MnemosyneDomain.Test.Queries.Notebooks
{
    public class GetNotebooksHandler
    {
        private FakeNotebookRepository _notebookRepository;
        private NotebookQueryHandler _handler;
        private Guid _userId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            _notebookRepository = new FakeNotebookRepository();
 
            _handler = new NotebookQueryHandler(
                _notebookRepository
            );

            // Pre-populate the repository with notebooks for testing
            _notebookRepository.Notebooks.Add(new Models.Notebook
            {
                NotebookId = 1,
                UserId = _userId,
                Title = "Test Notebook",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            });

            _notebookRepository.Notebooks.Add(new Models.Notebook
            {
                NotebookId = 2,
                UserId = Guid.NewGuid(),
                Title = "Test Notebook",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            });

            _notebookRepository.Notebooks.Add(new Models.Notebook
            {
                NotebookId = 3,
                UserId = _userId,
                Title = "Test Notebook",
                Created = DateTime.UtcNow,
                Updated = DateTime.UtcNow
            });
        }

        [Test]
        public async Task ShouldReturnAllOfAUsersNotebooks()
        {
            var query = new GetNotebooks(new User(_userId));

            var result = await _handler.HandleAsync(query);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2)); // Should return 2 notebooks for the user
        }
    }
}
