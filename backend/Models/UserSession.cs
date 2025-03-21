using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class UserSession
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string IpAddress { get; set; } = null!;

    public string Device { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
