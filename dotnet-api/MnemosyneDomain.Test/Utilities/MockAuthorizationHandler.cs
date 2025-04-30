using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;
using MnemosyneDomain.Authorization.Requirements;
using Moq;

namespace MnemosyneDomain.Test.Utilities
{
    public static class MockAuthorizationHandler
    {
        public static Mock<IAuthorizationHandler> GetAlwaysAuthorizedMock()
        {
            var authHandlerMock = new Mock<IAuthorizationHandler>();

            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<It.IsAnyType>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(true);
            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<Guid>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(true);
            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(true);

            return authHandlerMock;
        }

        public static Mock<IAuthorizationHandler> GetAlwaysUnauthorizedMock()
        {
            var authHandlerMock = new Mock<IAuthorizationHandler>();

            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<It.IsAnyType>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(false);
            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<Guid>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(false);
            authHandlerMock.Setup(h => h.IsAuthorizedAsync(It.IsAny<User>(), It.IsAny<int>(), It.IsAny<List<IAuthorizationRequirement<It.IsAnyType>>>()))
                .ReturnsAsync(false);

            return authHandlerMock;
        }
    }
}
