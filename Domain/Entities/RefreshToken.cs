namespace Domain.Entities;

public class RefreshToken
{
    public int Id { get; set; }

    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedByIp { get; set; } = string.Empty;

    public DateTime? RevokedAt { get; set; }
    public string? RevokedByIp { get; set; }
    public string? ReplacedByToken { get; set; }

    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsActive => RevokedAt == null && !IsExpired;

    public string UserId { get; set; } = string.Empty;
}
