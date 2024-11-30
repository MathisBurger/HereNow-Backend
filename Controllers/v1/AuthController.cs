using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;
using PresenceBackend.Models.Response;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

[Route("v1/auth")]
[ApiController]
public class AuthController: ControllerBase
{

    private readonly DbAccess Db;

    public AuthController(DbAccess db)
    {
        Db = db;
    }

    [AllowAnonymous]
    [HttpGet("")]
    public IActionResult AuthInfo()
    {
        return new OkObjectResult(new DefaultResponseModel("OK", "v1 authorization is enabled", "v1.0.0"));
    }

    [HttpPost("register")]
    [RateLimitFilter(5, 10)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        User user = await this.Db.UserRepository.RegisterUser(registerRequest);
        return Ok(user);
    }
    
}