namespace Life4DunBackend.Core.DTOs;

/// <summary>
/// DTO cho thông tin người chơi
/// </summary>
public class PlayerDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public long Experience { get; set; }
    public long Gold { get; set; }
    public long Gems { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public string? AvatarUrl { get; set; }
    public PlayerAttributesDto Attributes { get; set; } = new();
}

public class PlayerAttributesDto
{
    public int Level { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int Speed { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
}

/// <summary>
/// DTO cho tạo người chơi mới
/// </summary>
public class CreatePlayerDto
{
    /// <summary>
    /// ID của tài khoản User sở hữu nhân vật
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    public Guid UserId { get; set; }

    /// <summary>
    /// Tên nhân vật (Username) muốn tạo
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MinLength(3)]
    [System.ComponentModel.DataAnnotations.MaxLength(50)]
    public string Username { get; set; } = string.Empty;
}

/// <summary>
/// DTO cho tạo/đăng ký User
/// </summary>
public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
}

/// <summary>
/// DTO cho đăng nhập
/// </summary>
public class LoginDto
{
    /// <summary>
    /// Email đăng ký hoặc Tên nhân vật (Username) của người chơi
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    public string UsernameOrEmail { get; set; } = string.Empty;

    /// <summary>
    /// Mật khẩu của tài khoản
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// DTO cho response đăng nhập
/// </summary>
public class AuthResponseDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// DTO cho User
/// </summary>
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public bool IsActive { get; set; }
    public List<PlayerDto> Players { get; set; } = new();
}

/// <summary>
/// DTO chứa thông tin đăng ký tài khoản User và khởi tạo Player ban đầu từ Game Client
/// </summary>
public class GameRegisterDto
{
    /// <summary>
    /// Địa chỉ email đăng ký (dùng để đăng nhập)
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Mật khẩu tài khoản (tối thiểu 6 ký tự)
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MinLength(6)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Tên đầy đủ của chủ tài khoản
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Tên nhân vật (Username) khởi tạo ban đầu cho Game
    /// </summary>
    [System.ComponentModel.DataAnnotations.Required]
    [System.ComponentModel.DataAnnotations.MinLength(3)]
    [System.ComponentModel.DataAnnotations.MaxLength(50)]
    public string PlayerUsername { get; set; } = string.Empty;
}

/// <summary>
/// DTO phản hồi sau khi đăng ký thành công tài khoản và khởi tạo nhân vật
/// </summary>
public class GameRegisterResponseDto
{
    /// <summary>
    /// ID tài khoản User vừa tạo
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Email của User
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Tên đầy đủ của User
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Nhân vật (Player) đầu tiên vừa được khởi tạo
    /// </summary>
    public PlayerDto InitialPlayer { get; set; } = new();
}

