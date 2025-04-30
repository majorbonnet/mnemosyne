using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Models;
using MnemosyneDomain.Repositories;
using Moq;

namespace MnemosyneDomain.Test.Authorization
{
    public class IsAuthorizedAsync
    {
        private IRepository<UserInfo> _repository;
        private IRepositoryFactory _repositoryFactory;
        private AuthorizationHandler _authorizationHandler;
        private Guid _userId1 = Guid.NewGuid();
        private Guid _userId2 = Guid.NewGuid();

        private Guid _pageId1 = Guid.NewGuid();
        private Guid _pageId2 = Guid.NewGuid();
        private Guid _pageId3 = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            var userRepoMock = new Mock<IRepository<UserInfo>>();
            var notebookRepoMock = new Mock<IRepository<Notebook>>();
            var notebookPageRepoMock = new Mock<IRepository<NotebookPage>>();

            notebookRepoMock.Setup(r => r.FindOneAsync(1))
                .ReturnsAsync((object[] keyValues) => new Notebook { NotebookId = (int)keyValues[0], UserId = _userId1, Title = "Test Notebook" });

            notebookRepoMock.Setup(r => r.FindOneAsync(2))
                .ReturnsAsync((object[] keyValues) => new Notebook { NotebookId = (int)keyValues[0], UserId = _userId2, Title = "Test Notebook" });

            notebookPageRepoMock.Setup(r => r.FindOneAsync(_pageId1))
                .ReturnsAsync((object[] keyValues) => new NotebookPage
                {
                    NotebookPageId = (Guid)keyValues[0],
                    NotebookId = 1,
                    Notebook = new Notebook { NotebookId = 1, UserId = _userId1, Title = "Test Notebook" },
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page 1",
                    Contents = "This is a test page."
                });

            notebookPageRepoMock.Setup(r => r.FindOneAsync(_pageId2))
                .ReturnsAsync((object[] keyValues) => new NotebookPage
                {
                    NotebookPageId = (Guid)keyValues[0],
                    NotebookId = 2,
                    Notebook = new Notebook { NotebookId = 2, UserId = _userId2, Title = "Test Notebook" },
                    PageNumber = 0,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow,
                    Title = "Test Page 2",
                    Contents = "This is a test page."
                });

            var repoFactoryMock = new Mock<IRepositoryFactory>();
            repoFactoryMock.Setup(r => r.CreateRepository<Notebook>())
                .Returns(notebookRepoMock.Object);
            repoFactoryMock.Setup(r => r.CreateRepository<NotebookPage>())
                .Returns(notebookPageRepoMock.Object);

            _repository = userRepoMock.Object;
            _repositoryFactory = repoFactoryMock.Object;

            _authorizationHandler = new AuthorizationHandler(
                _repository,
                _repositoryFactory
            );
        }

        [Test]
        public async Task ShouldReturnTrueIfUserMeetsSpecsForIntId()
        {   
            var isAuthorized = await _authorizationHandler.IsAuthorizedAsync(new User(_userId1), 1, AuthorizationPolicies.NotebookOwner);

            Assert.That(isAuthorized, Is.True);
        }

        [Test]
        public async Task ShouldReturnFalseIfUserDoesNotMeetSpecsForIntId()
        {
            var isAuthorized = await _authorizationHandler.IsAuthorizedAsync(new User(_userId2), 1, AuthorizationPolicies.NotebookOwner);

            Assert.That(isAuthorized, Is.False);
        }

        [Test]
        public async Task ShouldReturnTrueIfUserMeetsSpecsForGuid()
        {
            var isAuthorized = await _authorizationHandler.IsAuthorizedAsync(new User(_userId1), _pageId1, AuthorizationPolicies.NotebookPageOwner);

            Assert.That(isAuthorized, Is.True);
        }

        [Test]
        public async Task ShouldReturnFalseIfUserDoesNotMeetSpecsForGuid()
        {
            var isAuthorized = await _authorizationHandler.IsAuthorizedAsync(new User(_userId2), _pageId1, AuthorizationPolicies.NotebookPageOwner);

            Assert.That(isAuthorized, Is.False);
        }
    }
}
