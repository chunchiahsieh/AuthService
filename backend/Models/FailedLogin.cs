using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class FailedLogin
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string IpAddress { get; set; } = null!;

    public DateTime? AttemptAt { get; set; }
}
