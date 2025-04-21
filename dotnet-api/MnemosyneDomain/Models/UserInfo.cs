using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class UserInfo
{
    public Guid UserId { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Notebook> Notebooks { get; set; } = new List<Notebook>();
}
