using Microsoft.EntityFrameworkCore;
using MnemosyneDomain.Authorization.Requirements;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Authorization
{
    public class AuthorizationHandler
    {
        private readonly MnemosyneContext _context;
        private User? _user;

        public AuthorizationHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task HandleAsync(VerifyUser request)
        {
            UserInfo? user = _context.UserInfos.FirstOrDefault(x => x.UserId == request.UserId);

            if (user is null)
            {
                user = new()
                {
                    UserId = request.UserId
                };

                _context.UserInfos.Add(user);

                Notebook defaultNotebook = new()
                {
                    UserId = request.UserId,
                    Created = DateTime.UtcNow,
                    Updated = DateTime.UtcNow
                };

                _context.Notebooks.Add(defaultNotebook);

                await _context.SaveChangesAsync();
            }
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
        public bool IsAuthorized<TResource>(User user, Guid resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            if (user is null) return false;

            TResource? resource = _context.Set<TResource>().Find(resourceId);

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
        public bool IsAuthorized<TResource>(User user, int resourceId, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            if (user is null) return false;

            TResource? resource = _context.Set<TResource>().Find(resourceId);

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
        public bool IsAuthorized<TResource>(User user, TResource resource, List<IAuthorizationRequirement<TResource>> requirements) where TResource : class
        {
            if (user is null) return false;

            foreach (var requirement in requirements)
            {
                if (!requirement.IsMet(user, resource))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
