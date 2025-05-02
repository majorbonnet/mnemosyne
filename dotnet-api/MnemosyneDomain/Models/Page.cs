using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class Page
{
    public Guid PageId { get; set; }

    public Guid NotebookId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public int PageNumber { get; set; }

    public string? Title { get; set; }

    public string? Contents { get; set; }

    public virtual Notebook Notebook { get; set; } = null!;
}
