using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Models.Response;

namespace PresenceBackend.Controllers;

/// <summary>
/// Default controller
/// </summary>
[ApiController]
public class DefaultController
{
    
    /// <summary>
    /// Gets the default response
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("/")]
    public IActionResult Default()
    {
        return new OkObjectResult(new DefaultResponseModel("RUNNING", "Backend service is up and running", "v1.0.0"));
    }
}