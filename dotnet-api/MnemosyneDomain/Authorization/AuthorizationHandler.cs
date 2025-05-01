using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Commands.Notebooks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Authorization
{
    public class AuthorizationHandler : IAuthorizationHandler
    {
        private readonly MnemosyneContext _context;

        public AuthorizationHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task<CreateNotebook?> HandleAsync(VerifyUser request)
        {
            if (!await _context.UserInfos.AnyAsync(u => u.UserId == request.UserId))
            {
                _context.UserInfos.Add(new UserInfo { UserId = request.UserId });
                await _context.SaveChangesAsync();

                return new CreateNotebook(new User(request.UserId));
            }

            return null;
        }

        /// <summary>
        /// Check if the user is authorized to access a resource based on requirements. Use this overload in situations where the resource is not needed outside of the authz check,
        /// such as when checking for ability to interact with a sub-resource
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="user"></param>
        /// <param name="resourceId"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public async Task<bool> IsAuthorizedAsync<TResource>(User user, Guid resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            if (user is null) return false;

            TResource? resource = await _context.Set<TResource>().FindAsync(resourceId);

            if (resource is null) return false;

            foreach (var requirement in requirements)
            {
                if (!requirement.IsMet(user, resource))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if the user is authorized to access a resource based on requirements. Use this overload in situations where the resource is not needed outside of the authz check,
        /// such as when checking for ability to interact with a sub-resource
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="user"></param>
        /// <param name="resourceId"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public async Task<bool> IsAuthorizedAsync<TResource>(User user, int resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            if (user is null) return false;

            TResource? resource = await _context.Set<TResource>().FindAsync(resourceId);

            if (resource is null) return false;

            foreach (var requirement in requirements)
            {
                if (!requirement.IsMet(user, resource))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if the user is authorized to access a resource based on requirements. Use this overload in situations where the resource is already loaded in memory, 
        /// ie when the resource is needed outside the context of the authz check.
        /// </summary>
        /// <typeparam name="TResource"></typeparam>
        /// <param name="user"></param>
        /// <param name="resource"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public Task<bool> IsAuthorizedAsync<TResource>(User user, TResource resource, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            if (user is null) return Task.FromResult(false);

            foreach (var requirement in requirements)
            {
                if (!requirement.IsMet(user, resource))
                {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }
    }
}
