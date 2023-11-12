using Microsoft.AspNetCore.Mvc;

namespace RobotCleaner.Api.Features.Clean;

[ApiController]
[Route("tibber-developer-test")]
public class CleanController : ControllerBase
{
    private readonly IClean _clean;

    public CleanController(IClean clean)
    {
        _clean = clean;
    }

    [HttpPost("enter-path")]
    [ProducesResponseType(typeof(IClean.Result), StatusCodes.Status200OK, "application/json")]
    public async Task<IActionResult> EnterPath(IClean.Request request)
    {
        var result = await _clean.ExecuteAsync(request);
        return Ok(result);
    }
}