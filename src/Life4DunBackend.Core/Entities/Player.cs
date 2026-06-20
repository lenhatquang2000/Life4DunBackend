namespace Life4DunBackend.Core.Entities;

/// <summary>
/// Đại diện cho một nhân vật/người chơi trong game của một User
/// </summary>
public class Player
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public long Experience { get; set; }
    public long Gold { get; set; }
    public long Gems { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public string? AvatarUrl { get; set; }
    public string Model { get; set; } = "Mira";
    
    // Consolidate attributes/stats into JSON column
    public PlayerAttributes Attributes { get; set; } = new();
    
    // Navigation property
    public User? User { get; set; }
}

public class PlayerAttributes
{
    public int Level { get; set; } = 1;
    public int Attack { get; set; } = 10;
    public int Defense { get; set; } = 5;
    public int Speed { get; set; } = 10;
    public int Health { get; set; } = 100;
    public int MaxHealth { get; set; } = 100;
}

