using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MnemosyneDomain.Authorization;
using Wolverine;
using Wolverine.Http;

namespace MnemosyneApi.Middleware
{
    public static class VerifyUserMiddleware
    {
        public static async Task<(User, ProblemDetails)> Load(IMessageBus bus, ClaimsPrincipal user)
        {
            string userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedUserId))
            {
                await bus.InvokeAsync(new VerifyUser(parsedUserId));

                return (new User(parsedUserId), WolverineContinue.NoProblems);
            }

            return (new User(Guid.Empty), new ProblemDetails { Detail = "Unauthorized", Status = 401 });
        }
    }
}
