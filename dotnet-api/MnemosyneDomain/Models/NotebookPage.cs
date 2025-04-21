using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class NotebookPage
{
    public Guid NotebookPageId { get; set; }

    public int NotebookId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public int PageNumber { get; set; }

    public string? Title { get; set; }

    public string? Contents { get; set; }

    public virtual Notebook Notebook { get; set; } = null!;
}
