using Life4DunBackend.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Life4DunBackend.Infrastructure.Data;

/// <summary>
/// DbContext chính cho ứng dụng game
/// </summary>
public class GameDbContext : DbContext
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Player> Players { get; set; } = null!;
    public DbSet<GameSession> GameSessions { get; set; } = null!;
    public DbSet<RolePermission> RolePermissions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure User entity
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.PasswordHash)
                .IsRequired();
            
            entity.Property(e => e.FullName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasIndex(e => e.Email).IsUnique();
            
            // One User can have many Players
            entity.HasMany(e => e.Players)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Set default values
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Role)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Player");
            // Don't set default value for CreatedAt, will be set by application
        });

        // Configure Player entity
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.UserId).IsRequired();
            
            entity.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.UserId);
            
            // Set default values for MySQL
            entity.Property(e => e.Experience).HasDefaultValue(0);
            entity.Property(e => e.Gold).HasDefaultValue(100);
            entity.Property(e => e.Gems).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            
            // Map attributes to a JSON column
            entity.Property(e => e.Attributes)
                .HasColumnType("json");
            // Don't set default value for CreatedAt, will be set by application
        });

        // Configure GameSession entity
        modelBuilder.Entity<GameSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("InProgress");
            
            entity.HasIndex(e => e.PlayerId);
            entity.HasIndex(e => e.StartedAt);
            
            // Set default values
            entity.Property(e => e.Score).HasDefaultValue(0);
            entity.Property(e => e.WavesCompleted).HasDefaultValue(0);
            entity.Property(e => e.RewardGold).HasDefaultValue(0);
            entity.Property(e => e.RewardExperience).HasDefaultValue(0);
            entity.Property(e => e.EnemiesDefeated).HasDefaultValue(0);
        });

        // Configure RolePermission entity
        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.RoleName)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.Resource)
                .IsRequired()
                .HasMaxLength(100);
                
            entity.HasIndex(e => e.RoleName);
        });
    }
}
