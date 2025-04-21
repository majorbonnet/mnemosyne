using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Models;

namespace MnemosyneDomain.Commands.Users
{
    public class UserCommandHandler : ICommandHandler<CreateUserIfNotExistsRequest>
    {
        private readonly MnemosyneContext _context;

        public UserCommandHandler(MnemosyneContext context)
        {
            _context = context;
        }

        public async Task Handle(CreateUserIfNotExistsRequest request)
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
    }
}
