using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Services;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

/// <summary>
/// Controller to handle protocol related actions
/// </summary>
[ApiController]
[Route("v1/protocols")]
[TypeFilter(typeof(AuthorizationFilter))]
public class ProtocolController : AuthorizedControllerBase
{
    private readonly DbAccess _db;
    private readonly MailService _mailService;

    public ProtocolController(DbAccess db, MailService mailService)
    {
        _db = db;
        _mailService = mailService;
    }
    
    /// <summary>
    /// Gets all emergencies
    /// </summary>
    /// <returns>All emergencies</returns>
    [HttpGet("emergencies")]
    public async Task<IActionResult> GetEmergencies()
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        return Ok(await this._db.ProtocolRepository.FindAll(ProtocolAction.Emergency));
    }
    
    /// <summary>
    /// Gets an emergency
    /// </summary>
    /// <param name="id">The ID of the emergency</param>
    /// <returns>The emergency</returns>
    [HttpGet("emergencies/{id}")]
    public async Task<IActionResult> GetEmergency(Guid id)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        return Ok(await this._db.ProtocolRepository.FindOneById(id));
    }

    /// <summary>
    /// Creates an emergency
    /// </summary>
    /// <returns>The created emergency</returns>
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

        foreach (var admin in await this._db.UserRepository.FindAllAdmins())
        {
            this._mailService.SendEmailAsync(admin.Email, "Notfall angelegt",
                $"Es wurde ein Notfall mit {protocol.InvolvedUsers.Count()} Nutzern erstellt. Zeitpunkt: {DateTime.Now}");
        }
        return Ok(protocol);
    }
}