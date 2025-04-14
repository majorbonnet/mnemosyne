using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class UserInfo
{
    public Guid UserId { get; set; }

    public string? DisplayName { get; set; }

    public DateTime? LastLogin { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Journal> Journals { get; set; } = new List<Journal>();
}
