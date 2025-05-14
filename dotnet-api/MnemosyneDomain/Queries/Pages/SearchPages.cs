using MnemosyneDomain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.Pages
{
    public class SearchPages(User user, string query) : BaseRequest
    {
        public User User => user;
        public string Query => query;
        public bool IsExactMatch => query.StartsWith("\"") && query.EndsWith("\"");
    }
}
