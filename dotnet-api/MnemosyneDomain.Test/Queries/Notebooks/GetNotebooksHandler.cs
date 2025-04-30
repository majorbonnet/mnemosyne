using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Queries.Notebooks;
using MnemosyneDomain.Repositories;
using Moq;

namespace MnemosyneDomain.Test.Queries.Notebooks
{
    public class GetNotebooksHandler
    {
        private Guid _userId = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ShouldReturnAllOfAUsersNotebooks()
        {
            var notebooks = new List<Models.Notebook>
            {
                new Models.Notebook
                {
                    NotebookId = 1,
                    UserId = _userId,
                    Title = "Test Notebook",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                },
                new Models.Notebook
                {
                    NotebookId = 2,
                    UserId = Guid.NewGuid(),
                    Title = "Test Notebook",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                },
                new Models.Notebook
                {
                    NotebookId = 3,
                    UserId = _userId,
                    Title = "Test Notebook",
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                }
            };

            var repositoryMock = new Mock<IRepository<Models.Notebook>>();

            repositoryMock.Setup(r => r.Find(It.IsAny<Expression<Func<Models.Notebook, bool>>>()))
                .Returns((Expression<Func<Models.Notebook, bool>> predicate) =>
                    notebooks.Where(predicate.Compile()).AsQueryable());

            var handler = new NotebookQueryHandler(
                repositoryMock.Object
            );

            var query = new GetNotebooks(new User(_userId));

            var result = handler.Handle(query);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(2)); // Should return 2 notebooks for the user
        }
    }
}
