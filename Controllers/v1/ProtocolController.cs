using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

[ApiController]
[Route("v1/protocols")]
[TypeFilter(typeof(AuthorizationFilter))]
public class ProtocolController : AuthorizedControllerBase
{
    private readonly DbAccess _db;

    public ProtocolController(DbAccess db)
    {
        _db = db;
    }
    
    [HttpGet("emergencies")]
    public async Task<IActionResult> GetEmergencies()
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        return Ok(await this._db.ProtocolRepository.FindAll(ProtocolAction.Emergency));
    }
    
    [HttpGet("emergencies/{id}")]
    public async Task<IActionResult> GetEmergency(Guid id)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        return Ok(await this._db.ProtocolRepository.FindOneById(id));
    }

    [HttpPost("emergencies")]
    public async Task<IActionResult> CreateEmergency()
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Member))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        Protocol protocol = new Protocol() {Creator = this.CurrentUser, Action = ProtocolAction.Emergency};
        protocol.InvolvedUsers = await this._db.UserRepository.FindCurrentClockedIn();
        this._db.EntityManager.Add(protocol);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok(protocol);
    }
}