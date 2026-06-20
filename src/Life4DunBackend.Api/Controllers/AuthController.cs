using Life4DunBackend.Core.DTOs;
using Life4DunBackend.Core.Entities;
using Life4DunBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Life4DunBackend.Api.Controllers;

/// <summary>
/// API Controller xử lý đăng ký và đăng nhập tài khoản User.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly GameDbContext _context;
    private readonly ILogger<AuthController> _logger;

    public AuthController(GameDbContext context, ILogger<AuthController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Đăng ký tài khoản User mới (chưa tạo nhân vật/player).
    /// </summary>
    /// <remarks>
    /// Gửi thông tin Email, Mật khẩu và Tên đầy đủ để tạo tài khoản User.
    /// Sau khi có tài khoản User, Client có thể gọi API tạo Player để khởi tạo nhân vật.
    /// </remarks>
    /// <param name="registerDto">Thông tin đăng ký tài khoản (Email, Mật khẩu, Tên đầy đủ)</param>
    /// <returns>Thông tin tài khoản User vừa tạo thành công</returns>
    /// <response code="200">Đăng ký tài khoản thành công</response>
    /// <response code="400">Email đã được sử dụng trước đó, hoặc dữ liệu không hợp lệ</response>
    /// <response code="500">Lỗi máy chủ trong quá trình xử lý</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        try
        {
            // 1. Kiểm tra dữ liệu đầu vào cơ bản
            if (string.IsNullOrWhiteSpace(registerDto.Email) || 
                string.IsNullOrWhiteSpace(registerDto.Password) || 
                string.IsNullOrWhiteSpace(registerDto.FullName))
            {
                return BadRequest(new { message = "Email, Password và FullName không được bỏ trống." });
            }

            if (registerDto.Password.Length < 6)
            {
                return BadRequest(new { message = "Mật khẩu phải có tối thiểu 6 ký tự." });
            }

            // 2. Kiểm tra trùng lặp email
            var emailExists = await _context.Users.AnyAsync(u => u.Email == registerDto.Email);
            if (emailExists)
            {
                return BadRequest(new { message = "Email đã tồn tại trong hệ thống." });
            }

            // 3. Khởi tạo User và mã hóa mật khẩu
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                FullName = registerDto.FullName,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                IsActive = true
            };

            // 4. Lưu vào cơ sở dữ liệu
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // 5. Trả về thông tin UserDto
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsActive = user.IsActive,
                Players = new List<PlayerDto>()
            };

            _logger.LogInformation($"Tài khoản User {user.Email} đã được đăng ký thành công.");
            return Ok(userDto);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Lỗi xảy ra khi đăng ký User: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi hệ thống khi xử lý đăng ký." });
        }
    }
}
