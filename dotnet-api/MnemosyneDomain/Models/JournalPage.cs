using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class JournalPage
{
    public Guid JournalPageId { get; set; }

    public int JournalId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public int PageNumber { get; set; }

    public string? Title { get; set; }

    public string? Contents { get; set; }

    public virtual Journal Journal { get; set; } = null!;
}
