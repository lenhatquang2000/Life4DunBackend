namespace Life4DunBackend.Core.Entities;

/// <summary>
/// Đại diện cho một người dùng trong hệ thống
/// </summary>
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string Role { get; set; } = "Player";
    
    // Navigation property
    public ICollection<Player> Players { get; set; } = new List<Player>();
}
