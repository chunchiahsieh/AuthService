using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class AuthToken
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime ExpiresAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
