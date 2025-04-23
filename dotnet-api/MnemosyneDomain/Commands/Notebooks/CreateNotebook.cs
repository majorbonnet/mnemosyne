using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MnemosyneDomain.Authorization;

namespace MnemosyneDomain.Commands.Notebooks
{
    public class CreateNotebook(User user) : BaseRequest
    {
        public User User => user;
    }
}
