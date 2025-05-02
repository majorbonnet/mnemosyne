using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MnemosyneApi.Extensions;
using MnemosyneDomain.Authorization;
using Wolverine;
using Wolverine.Http;

namespace MnemosyneApi.Middleware
{
    public static class VerifyUserMiddleware
    {
        public static async Task<(User, ProblemDetails)> Load(IMessageBus bus, ClaimsPrincipal user)
        {
            if (user.GetUserId() is Guid userId)
            {
                await bus.InvokeAsync(new VerifyUser(userId));

                return (new User(userId), WolverineContinue.NoProblems);
            }

            return (new User(Guid.Empty), new ProblemDetails { Detail = "Unauthorized", Status = 401 });
        }
    }
}
