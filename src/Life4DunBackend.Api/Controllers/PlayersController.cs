using Life4DunBackend.Core.DTOs;
using Life4DunBackend.Core.Entities;
using Life4DunBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Life4DunBackend.Api.Controllers;

/// <summary>
/// API Controller để quản lý thông tin người chơi
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly GameDbContext _context;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(GameDbContext context, ILogger<PlayersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách tất cả người chơi
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
    {
        try
        {
            var players = await _context.Players
                .Select(p => new PlayerDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Username = p.Username,
                    Experience = p.Experience,
                    Gold = p.Gold,
                    Gems = p.Gems,
                    CreatedAt = p.CreatedAt,
                    LastLoginAt = p.LastLoginAt,
                    AvatarUrl = p.AvatarUrl,
                    Model = p.Model,
                    Attributes = new PlayerAttributesDto
                    {
                        Level = p.Attributes.Level,
                        Attack = p.Attributes.Attack,
                        Defense = p.Attributes.Defense,
                        Speed = p.Attributes.Speed,
                        Health = p.Attributes.Health,
                        MaxHealth = p.Attributes.MaxHealth
                    }
                })
                .ToListAsync();

            return Ok(players);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting players: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Lấy thông tin người chơi theo ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<PlayerDto>> GetPlayer(Guid id)
    {
        try
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound(new { message = "Player not found" });
            }

            var playerDto = new PlayerDto
            {
                Id = player.Id,
                UserId = player.UserId,
                Username = player.Username,
                Experience = player.Experience,
                Gold = player.Gold,
                Gems = player.Gems,
                CreatedAt = player.CreatedAt,
                LastLoginAt = player.LastLoginAt,
                AvatarUrl = player.AvatarUrl,
                Model = player.Model,
                Attributes = new PlayerAttributesDto
                {
                    Level = player.Attributes.Level,
                    Attack = player.Attributes.Attack,
                    Defense = player.Attributes.Defense,
                    Speed = player.Attributes.Speed,
                    Health = player.Attributes.Health,
                    MaxHealth = player.Attributes.MaxHealth
                }
            };

            return Ok(playerDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting player: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Lấy danh sách nhân vật (Players) theo UserId
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayersByUserId(Guid userId)
    {
        try
        {
            var players = await _context.Players
                .Where(p => p.UserId == userId)
                .Select(p => new PlayerDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Username = p.Username,
                    Experience = p.Experience,
                    Gold = p.Gold,
                    Gems = p.Gems,
                    CreatedAt = p.CreatedAt,
                    LastLoginAt = p.LastLoginAt,
                    AvatarUrl = p.AvatarUrl,
                    Model = p.Model,
                    Attributes = new PlayerAttributesDto
                    {
                        Level = p.Attributes.Level,
                        Attack = p.Attributes.Attack,
                        Defense = p.Attributes.Defense,
                        Speed = p.Attributes.Speed,
                        Health = p.Attributes.Health,
                        MaxHealth = p.Attributes.MaxHealth
                    }
                })
                .ToListAsync();

            return Ok(players);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting players by user id {userId}: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Khởi tạo nhân vật (Player) mới cho một User cụ thể.
    /// </summary>
    /// <remarks>
    /// Tạo một nhân vật trong game liên kết với tài khoản User.
    /// Nhân vật sẽ được khởi tạo với các thông số thuộc tính mặc định trong cột JSON Attributes.
    /// </remarks>
    /// <param name="createPlayerDto">Thông tin tạo nhân vật mới (UserId và Tên nhân vật)</param>
    /// <returns>Thông tin nhân vật vừa khởi tạo thành công</returns>
    /// <response code="201">Tạo nhân vật thành công</response>
    /// <response code="400">Tên nhân vật đã tồn tại, hoặc dữ liệu không hợp lệ</response>
    /// <response code="404">Không tìm thấy tài khoản User</response>
    /// <response code="500">Lỗi hệ thống</response>
    [HttpPost]
    [ProducesResponseType(typeof(PlayerDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PlayerDto>> CreatePlayer(CreatePlayerDto createPlayerDto)
    {
        try
        {
            // Kiểm tra User tồn tại
            var user = await _context.Users.FindAsync(createPlayerDto.UserId);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Kiểm tra username đã tồn tại
            if (await _context.Players.AnyAsync(p => p.Username == createPlayerDto.Username))
            {
                return BadRequest(new { message = "Username already exists" });
            }

            var player = new Player
            {
                Id = Guid.NewGuid(),
                UserId = createPlayerDto.UserId,
                Username = createPlayerDto.Username,
                Model = createPlayerDto.ModelName,
                Experience = 0,
                Gold = 100,
                Gems = 0,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                IsActive = true,
                Attributes = new PlayerAttributes
                {
                    Level = 1,
                    Health = 100,
                    MaxHealth = 100,
                    Attack = 10,
                    Defense = 5,
                    Speed = 10
                }
            };

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            var playerDto = new PlayerDto
            {
                Id = player.Id,
                UserId = player.UserId,
                Username = player.Username,
                Model = player.Model,
                Experience = player.Experience,
                Gold = player.Gold,
                Gems = player.Gems,
                CreatedAt = player.CreatedAt,
                LastLoginAt = player.LastLoginAt,
                Attributes = new PlayerAttributesDto
                {
                    Level = player.Attributes.Level,
                    Attack = player.Attributes.Attack,
                    Defense = player.Attributes.Defense,
                    Speed = player.Attributes.Speed,
                    Health = player.Attributes.Health,
                    MaxHealth = player.Attributes.MaxHealth
                }
            };

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, playerDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating player: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Cập nhật thông tin người chơi
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlayer(Guid id, PlayerDto updatePlayerDto)
    {
        try
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound(new { message = "Player not found" });
            }

            player.Experience = updatePlayerDto.Experience;
            player.Gold = updatePlayerDto.Gold;
            player.Gems = updatePlayerDto.Gems;
            if (updatePlayerDto.Attributes != null)
            {
                player.Attributes.Level = updatePlayerDto.Attributes.Level;
                player.Attributes.Health = updatePlayerDto.Attributes.Health;
                player.Attributes.MaxHealth = updatePlayerDto.Attributes.MaxHealth;
                player.Attributes.Attack = updatePlayerDto.Attributes.Attack;
                player.Attributes.Defense = updatePlayerDto.Attributes.Defense;
                player.Attributes.Speed = updatePlayerDto.Attributes.Speed;
            }

            _context.Players.Update(player);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Player updated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating player: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Xóa người chơi
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlayer(Guid id)
    {
        try
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound(new { message = "Player not found" });
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting player: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Lấy bảng xếp hạng (Top 10)
    /// </summary>
    [HttpGet("leaderboard/top")]
    public async Task<ActionResult<IEnumerable<PlayerDto>>> GetLeaderboard()
    {
        try
        {
            var topPlayers = await _context.Players
                .OrderByDescending(p => p.Attributes.Level)
                .ThenByDescending(p => p.Experience)
                .Take(10)
                .Select(p => new PlayerDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    Username = p.Username,
                    Experience = p.Experience,
                    Gold = p.Gold,
                    Gems = p.Gems,
                    CreatedAt = p.CreatedAt,
                    LastLoginAt = p.LastLoginAt,
                    Model = p.Model,
                    Attributes = new PlayerAttributesDto
                    {
                        Level = p.Attributes.Level,
                        Attack = p.Attributes.Attack,
                        Defense = p.Attributes.Defense,
                        Speed = p.Attributes.Speed,
                        Health = p.Attributes.Health,
                        MaxHealth = p.Attributes.MaxHealth
                    }
                })
                .ToListAsync();

            return Ok(topPlayers);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting leaderboard: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
