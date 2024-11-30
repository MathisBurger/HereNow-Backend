using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Models.Response;

namespace PresenceBackend.Controllers;

[ApiController]
public class DefaultController
{
    
    [AllowAnonymous]
    [HttpGet("/")]
    public IActionResult Default()
    {
        return new OkObjectResult(new DefaultResponseModel("OK", "Backend service is up and running", "v1.0.0"));
    }
}