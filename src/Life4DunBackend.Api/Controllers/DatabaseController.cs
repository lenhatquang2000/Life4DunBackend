using Life4DunBackend.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace Life4DunBackend.Api.Controllers;

/// <summary>
/// API Controller để kiểm tra kết nối database
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DatabaseController : ControllerBase
{
    private readonly DbConnectionController _dbConnectionController;
    private readonly ILogger<DatabaseController> _logger;

    public DatabaseController(DbConnectionController dbConnectionController, ILogger<DatabaseController> logger)
    {
        _dbConnectionController = dbConnectionController;
        _logger = logger;
    }

    /// <summary>
    /// Kiểm tra kết nối database
    /// </summary>
    [HttpGet("health")]
    public async Task<IActionResult> HealthCheck()
    {
        try
        {
            var isConnected = await _dbConnectionController.TestConnectionAsync();
            
            if (isConnected)
            {
                return Ok(new { status = "Connected", message = "Database connection is healthy" });
            }
            else
            {
                return StatusCode(503, new { status = "Disconnected", message = "Cannot connect to database" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error checking database health: {ex.Message}");
            return StatusCode(503, new { status = "Error", message = ex.Message });
        }
    }

    /// <summary>
    /// Lấy thông tin database
    /// </summary>
    [HttpGet("info")]
    public async Task<IActionResult> GetDatabaseInfo()
    {
        try
        {
            var dbInfo = await _dbConnectionController.GetDatabaseInfoAsync();
            
            if (dbInfo == null)
            {
                return StatusCode(500, new { message = "Failed to retrieve database information" });
            }

            return Ok(dbInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error getting database info: {ex.Message}");
            return StatusCode(500, new { message = ex.Message });
        }
    }
}
