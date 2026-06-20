using Life4DunBackend.Core.Entities;
using Microsoft.Extensions.Logging;

namespace Life4DunBackend.Infrastructure.Data.Seeders;

/// <summary>
/// Seeder dùng để khởi tạo dữ liệu mẫu cho database
/// </summary>
public class DatabaseSeeder
{
    private readonly GameDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(GameDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Seed dữ liệu mẫu vào database
    /// </summary>
    public async Task SeedAsync()
    {
        try
        {
            // Kiểm tra xem đã có dữ liệu chưa
            if (_context.Users.Any())
            {
                _logger.LogInformation("Database already seeded");
                return;
            }

            _logger.LogInformation("Starting to seed database...");

            // Seed Users and Players
            await SeedUsersAndPlayersAsync();

            // Seed Game Sessions
            await SeedGameSessionsAsync();

            await _context.SaveChangesAsync();
            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error seeding database: {ex.Message}");
            throw;
        }
    }

    private async Task SeedUsersAndPlayersAsync()
    {
        var users = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                Email = "admin@life4dun.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FullName = "Administrator",
                CreatedAt = DateTime.UtcNow.AddMonths(-6),
                LastLoginAt = DateTime.UtcNow,
                IsActive = true,
                Players = new List<Player>
                {
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Username = "Admin",
                        Experience = 1000000,
                        Gold = 50000,
                        Gems = 1000,
                        CreatedAt = DateTime.UtcNow.AddMonths(-6),
                        LastLoginAt = DateTime.UtcNow,
                        IsActive = true,
                        Attributes = new PlayerAttributes
                        {
                            Level = 50,
                            Health = 100,
                            MaxHealth = 100,
                            Attack = 50,
                            Defense = 40,
                            Speed = 35
                        }
                    }
                }
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "player1@life4dun.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Player@123"),
                FullName = "Player One",
                CreatedAt = DateTime.UtcNow.AddMonths(-3),
                LastLoginAt = DateTime.UtcNow.AddDays(-1),
                IsActive = true,
                Players = new List<Player>
                {
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Username = "Player1",
                        Experience = 150000,
                        Gold = 10000,
                        Gems = 200,
                        CreatedAt = DateTime.UtcNow.AddMonths(-3),
                        LastLoginAt = DateTime.UtcNow.AddDays(-1),
                        IsActive = true,
                        Attributes = new PlayerAttributes
                        {
                            Level = 25,
                            Health = 80,
                            MaxHealth = 100,
                            Attack = 30,
                            Defense = 25,
                            Speed = 28
                        }
                    },
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Username = "Player1_Archer",
                        Experience = 50000,
                        Gold = 5000,
                        Gems = 100,
                        CreatedAt = DateTime.UtcNow.AddMonths(-2),
                        LastLoginAt = DateTime.UtcNow.AddDays(-5),
                        IsActive = true,
                        Attributes = new PlayerAttributes
                        {
                            Level = 15,
                            Health = 60,
                            MaxHealth = 100,
                            Attack = 35,
                            Defense = 15,
                            Speed = 40
                        }
                    }
                }
            },
            new User
            {
                Id = Guid.NewGuid(),
                Email = "player2@life4dun.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Player@456"),
                FullName = "Player Two",
                CreatedAt = DateTime.UtcNow.AddMonths(-1),
                LastLoginAt = DateTime.UtcNow.AddHours(-2),
                IsActive = true,
                Players = new List<Player>
                {
                    new Player
                    {
                        Id = Guid.NewGuid(),
                        Username = "Player2",
                        Experience = 80000,
                        Gold = 5000,
                        Gems = 100,
                        CreatedAt = DateTime.UtcNow.AddMonths(-1),
                        LastLoginAt = DateTime.UtcNow.AddHours(-2),
                        IsActive = true,
                        Attributes = new PlayerAttributes
                        {
                            Level = 18,
                            Health = 70,
                            MaxHealth = 100,
                            Attack = 20,
                            Defense = 18,
                            Speed = 22
                        }
                    }
                }
            }
        };

        await _context.Users.AddRangeAsync(users);
        _logger.LogInformation($"Seeded {users.Count} users");
    }

    private async Task SeedGameSessionsAsync()
    {
        var players = _context.Players.ToList();
        var sessions = new List<GameSession>();

        foreach (var player in players)
        {
            // Tạo 5 game session cho mỗi player
            for (int i = 0; i < 5; i++)
            {
                sessions.Add(new GameSession
                {
                    Id = Guid.NewGuid(),
                    PlayerId = player.Id,
                    StartedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)),
                    EndedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)).AddMinutes(Random.Shared.Next(5, 60)),
                    Status = "Completed",
                    Score = Random.Shared.Next(100, 5000),
                    WavesCompleted = Random.Shared.Next(1, 20),
                    RewardGold = Random.Shared.Next(100, 1000),
                    RewardExperience = Random.Shared.Next(50, 500),
                    DifficultyLevel = new[] { "Easy", "Normal", "Hard" }[Random.Shared.Next(3)],
                    EnemiesDefeated = Random.Shared.Next(10, 100),
                    DeviceInfo = "Android/iOS"
                });
            }
        }

        await _context.GameSessions.AddRangeAsync(sessions);
        _logger.LogInformation($"Seeded {sessions.Count} game sessions");
    }
}
