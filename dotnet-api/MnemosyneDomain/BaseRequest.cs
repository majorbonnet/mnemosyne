using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain
{
    public abstract class BaseRequest
    {
        public BaseRequest()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; protected set; }
    }
}
