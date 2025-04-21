using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class Image
{
    public Guid ImageId { get; set; }

    public Guid UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public string ImageKey { get; set; } = null!;

    public string? AltText { get; set; }

    public virtual UserInfo User { get; set; } = null!;
}
