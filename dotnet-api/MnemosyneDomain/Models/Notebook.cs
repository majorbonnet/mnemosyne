using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class Notebook
{
    public int NotebookId { get; set; }

    public Guid UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<NotebookPage> NotebookPages { get; set; } = new List<NotebookPage>();

    public virtual UserInfo User { get; set; } = null!;
}
