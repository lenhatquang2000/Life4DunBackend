using Life4DunBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Life4DunBackend.Api.Controllers;

/// <summary>
/// API Controller quản lý và truy vấn phân quyền RolePermission
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RolePermissionsController : ControllerBase
{
    private readonly GameDbContext _context;
    private readonly ILogger<RolePermissionsController> _logger;

    public RolePermissionsController(GameDbContext context, ILogger<RolePermissionsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Lấy danh sách các quyền (Resource) theo tên Role (RoleName)
    /// </summary>
    /// <param name="roleName">Tên của Role (ví dụ: admin, player)</param>
    /// <returns>Danh sách các resource/permission thuộc về role đó</returns>
    /// <response code="200">Trả về danh sách các permission</response>
    /// <response code="500">Lỗi hệ thống</response>
    [HttpGet("{roleName}")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<string>>> GetPermissionsByRole(string roleName)
    {
        try
        {
            var permissions = await _context.RolePermissions
                .Where(rp => rp.RoleName.ToLower() == roleName.ToLower())
                .Select(rp => rp.Resource)
                .ToListAsync();

            return Ok(permissions);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting permissions for role {roleName}: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Lỗi hệ thống khi tải phân quyền." });
        }
    }
}
