using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

[ApiController]
[TypeFilter(typeof(AuthorizationFilter))]
[Route("v1/users")]
public class UserController : AuthorizedControllerBase
{

    private readonly DbAccess _db;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserController(DbAccess db, IPasswordHasher<User> passwordHasher)
    {
        _db = db;
        _passwordHasher = passwordHasher;
    }

    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        return Ok(await this._db.UserRepository.FindAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        return Ok(await this._db.UserRepository.FindOneById(id));
    }

    [HttpGet("self")]
    public IActionResult Self()
    {
        return Ok(this.CurrentUser);
    }
    
    [HttpPost("self/newPassword")]
    public async Task<IActionResult> ResetPassword(Guid id, [FromBody] NewPasswordRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.Id == id)
        {
            return Unauthorized("Du kannst nur dein eigenes Passwort zurücksetzen");
        }
        
        User? user = await this._db.UserRepository.FindOneById(id);
        if (user == null)
        {
            return BadRequest("Nutzer konnte nicht gefunden werden");
        }

        if (_passwordHasher.VerifyHashedPassword(user, user.Password, request.OldPassword) ==
            PasswordVerificationResult.Failed)
        {
            return BadRequest("Die Passwörter stimmen nicht überein");
        }
        
        user.Password = _passwordHasher.HashPassword(user, request.NewPassword);
        this._db.EntityManager.Update(user);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok(user);
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] ChangeUserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(id);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        this._db.EntityManager.Update(user);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok(user);
    }
}