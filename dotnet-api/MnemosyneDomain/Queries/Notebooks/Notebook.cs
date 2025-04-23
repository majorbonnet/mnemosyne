using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MnemosyneDomain.Queries.Notebooks
{
    public record Notebook(int NotebookId, DateTime Created, DateTime Updated, string? Title);
}
