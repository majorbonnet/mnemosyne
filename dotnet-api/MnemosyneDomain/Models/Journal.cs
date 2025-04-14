using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class Journal
{
    public int JournalId { get; set; }

    public Guid UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<JournalPage> JournalPages { get; set; } = new List<JournalPage>();

    public virtual UserInfo User { get; set; } = null!;
}
