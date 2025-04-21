using System.Security.Claims;
using MnemosyneDomain.Commands;
using MnemosyneDomain.Commands.Users;

namespace MnemosyneApi.Middleware
{
    public class EnsureUserInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public EnsureUserInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

                // Create the user info in the database if it does not exist
                var command = new CreateUserIfNotExistsRequest(Guid.Parse(userId));
                var handler = context.RequestServices.GetRequiredService<UserCommandHandler>();
                await handler.Handle(command);
            }

            await _next(context);
        }
    }

    public static class EnsureUserInfoMiddlewareExtensions
    {
        public static IApplicationBuilder UseEnsureUserInfo(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<EnsureUserInfoMiddleware>();
        }
    }
}
