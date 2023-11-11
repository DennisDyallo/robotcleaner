using Microsoft.AspNetCore.Mvc;
using RobotCleaner.Api.Data;
using RobotCleaner.Api.Usecases.SaveCommands;

namespace RobotCleaner.Api.Controllers;

[ApiController]
[Route("tibber-developer-test")]
public class RobotCleanerController : ControllerBase
{

    private readonly ILogger<RobotCleanerController> _logger;
    private readonly ISaveCommands _saveCommands;

    public RobotCleanerController(ILogger<RobotCleanerController> logger, ISaveCommands saveCommands)
    {
        _logger = logger;
        _saveCommands = saveCommands;
    }

    [HttpPost("enter-path")]
    [ProducesResponseType(typeof(ISaveCommands.SaveCommandsResult), 200, "application/json")]
    public ActionResult<ISaveCommands.SaveCommandsResult> EnterPath(Request request)
    {
        var result = _saveCommands.ExecuteAsync(request);
        return Ok(result);
    }
}