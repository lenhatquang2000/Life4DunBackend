using Microsoft.AspNetCore.Mvc;

namespace Life4DunBackend.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    /// <summary>
    /// API lấy thông tin Session hiện tại của Người chơi.
    /// </summary>
    /// <param name="playerId">ID định danh của Player (UUID)</param>
    /// <response code="200">Trả về thông tin phiên chơi hợp lệ</response>
    /// <response code="404">Không tìm thấy phiên chơi nào</response>
    [HttpGet("session/{playerId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetSession(Guid playerId)
    {
        // Logic test debug nhanh
        return Ok(new { PlayerId = playerId, Status = "InProgress", Score = 9999 });
    }
}
