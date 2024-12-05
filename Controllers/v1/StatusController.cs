using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Response;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

[ApiController]
[Route("v1/status")]
[TypeFilter(typeof(AuthorizationFilter))]
public class StatusController : AuthorizedControllerBase
{
    private readonly DbAccess db;

    public StatusController(DbAccess db)
    {
        this.db = db;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetStatus()
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Member))
        {
            return Unauthorized("Du bist nicht berechtigt hierzu");
        }
        UserStatus? latest = await this.db.UserStatusRepository.GetLatestForUser(this.CurrentUser!);
            
        if (latest == null)
        {
            latest = new UserStatus();
            latest.Id = Guid.NewGuid();
            latest.Owner = this.CurrentUser;
            latest.ClockIn = DateTime.UtcNow;
        }

        return Ok(latest);
    }

    [HttpPost("toggle")]
    public async Task<IActionResult> ToggleStatus()
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Member))
        {
            return Unauthorized("Du bist hierzu nicht berechtigt");
        }
        
        UserStatus? userStatus = await this.db.UserStatusRepository.GetLatestForUser(this.CurrentUser!);
        if (userStatus == null || userStatus.ClockOut != null)
        {
            userStatus = new UserStatus();
            userStatus.ClockIn = DateTime.UtcNow;
            userStatus.Owner = this.CurrentUser;
            this.db.EntityManager.Add(userStatus);
            await this.db.EntityManager.SaveChangesAsync();
            return Ok(userStatus);
        }

        if (userStatus.ClockOut == null)
        {
            userStatus.ClockOut = DateTime.UtcNow;
            this.db.EntityManager.Update(userStatus);
            await this.db.EntityManager.SaveChangesAsync();
            return Ok(userStatus);
        }
        return BadRequest(new ErrorResponse("Etwas lief schief", StatusCodes.Status500InternalServerError));
    }
}