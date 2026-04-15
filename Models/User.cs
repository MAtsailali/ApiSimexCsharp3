using System;
using System.Collections.Generic;

namespace ApiSimexCsharp.Models;

public partial class User
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime? EmailVerifiedAt { get; set; }

    public string Password { get; set; } = null!;

    public string? RememberToken { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? TwoFactorSecret { get; set; }

    public string? TwoFactorRecoveryCodes { get; set; }

    public DateTime? TwoFactorConfirmedAt { get; set; }
}
