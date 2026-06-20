namespace Life4DunBackend.Core.Entities;

/// <summary>
/// Đại diện cho một phiên chơi game
/// </summary>
public class GameSession
{
    public Guid Id { get; set; }
    public Guid PlayerId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public string Status { get; set; } = "InProgress"; // InProgress, Completed, Abandoned
    public int Score { get; set; }
    public int WavesCompleted { get; set; }
    public long RewardGold { get; set; }
    public long RewardExperience { get; set; }
    public string? DifficultyLevel { get; set; }
    public int EnemiesDefeated { get; set; }
    public string? DeviceInfo { get; set; }
}
