using System;
using System.Collections.Generic;

namespace MnemosyneDomain.Models;

public partial class Image
{
    public Guid ImageId { get; set; }

    public Guid UserId { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }

    public string? AltText { get; set; }

    public string? FileLocation { get; set; }

    public virtual UserInfo User { get; set; } = null!;
}
