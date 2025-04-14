using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Commands
{
    public interface ICommandHandler<T> where T : class
    {
        Task Handle(T request);
    }
}
