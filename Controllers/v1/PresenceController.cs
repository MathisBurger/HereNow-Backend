using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

/// <summary>
/// Controller for handlung presences
/// </summary>
[ApiController]
[Route("v1/presence")]
[TypeFilter(typeof(AuthorizationFilter))]
public class PresenceController : AuthorizedControllerBase
{

    private readonly DbAccess _db;

    public PresenceController(DbAccess db)
    {
        _db = db;
    }

    /// <summary>
    /// Gets all active users
    /// </summary>
    /// <returns>All active users</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllActiveUsers()
    {
        if (this.CurrentUser != null && (
                                         this.CurrentUser.UserRoles.Contains(UserRole.Observer) ||
                                         this.CurrentUser.UserRoles.Contains(UserRole.Admin)))
        {
            return Ok(await this._db.UserRepository.FindCurrentClockedIn());
        }
        return Unauthorized("Du hast keine Rechte");
    }

    /// <summary>
    /// Logs out all users
    /// </summary>
    /// <returns>Ok result</returns>
    [HttpPost("logoutAll")]
    public async Task<IActionResult> LogoutAllActiveUsers()
    {
        if (this.CurrentUser != null && (this.CurrentUser.UserRoles.Contains(UserRole.KeyUser) ||
                                         this.CurrentUser.UserRoles.Contains(UserRole.Admin)))
        {
            List<UserStatus> statusList = await this._db.UserStatusRepository.GetAllLoggedIn();

            // The amount of data processed here will be not that big, because the 
            // app is scoped for a maximum of 100 people at the same time. Therefore, we 
            // can also update like this, which leaves the option to extend functionality (e.g. Sending Mails)
            foreach (UserStatus status in statusList)
            {
                status.ClockOut = DateTime.UtcNow;
                this._db.EntityManager.Update(status);
            }

            await this._db.EntityManager.SaveChangesAsync();

            Protocol protocol = new Protocol() { Action = ProtocolAction.LogoutAll, Creator = this.CurrentUser };
            protocol.InvolvedUsers = statusList.Select(s => s.Owner!).ToList();
            this._db.EntityManager.Add(protocol);
            await this._db.EntityManager.SaveChangesAsync();
            
            return Ok(protocol);
        }
        return Unauthorized();
    }

}