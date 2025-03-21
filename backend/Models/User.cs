using System;
using System.Collections.Generic;

namespace Api.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AuthToken> AuthTokens { get; set; } = new List<AuthToken>();

    public virtual ICollection<UserSession> UserSessions { get; set; } = new List<UserSession>();
}
