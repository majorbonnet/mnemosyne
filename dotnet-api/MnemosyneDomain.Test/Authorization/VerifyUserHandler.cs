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
    public class VerifyUserHandler
    {
        private IRepository<UserInfo> _repository;
        private IRepositoryFactory _repositoryFactory;
        private AuthorizationHandler _authorizationHandler;
        private Guid _userId1 = Guid.NewGuid();
        private Guid _userId2 = Guid.NewGuid();

        [SetUp]
        public void Setup()
        {
            var userRepoMock = new Mock<IRepository<UserInfo>>();
            var repoFactoryMock = new Mock<IRepositoryFactory>();

            var users = new List<UserInfo>
            {
                new UserInfo { UserId = _userId1 },
                new UserInfo { UserId = _userId2 }
            };

            userRepoMock.Setup(r => r.AddIfNotExistsAsync(It.IsAny<UserInfo>()))
                .ReturnsAsync((UserInfo userInfo) =>
                {
                    if (users.Any(user => user.UserId == userInfo.UserId))
                    {
                        return false; // User already exists
                    }

                    return true;
                });

            _repository = userRepoMock.Object;
            _repositoryFactory = repoFactoryMock.Object;

            _authorizationHandler = new AuthorizationHandler(
                _repository,
                _repositoryFactory
            );

        }

        [Test]
        public async Task ShouldReturnCreateNotebookIfUserWasAdded()
        {
            var command = new VerifyUser(Guid.NewGuid());

            var result = await _authorizationHandler.HandleAsync(command);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.User.UserId, Is.EqualTo(command.UserId));
        }

        [Test]
        public async Task ShouldReturnNullIfUserAlreadyExisted()
        {
            var command = new VerifyUser(_userId1);

            var result = await _authorizationHandler.HandleAsync(command);

            Assert.That(result, Is.Null);
        }
    }
}
