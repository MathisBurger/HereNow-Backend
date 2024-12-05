using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

/// <summary>
/// Controller for handling admin interactions
/// </summary>
[ApiController]
[Route("v1/admin")]
[TypeFilter(typeof(AuthorizationFilter))]
public class AdminController : AuthorizedControllerBase
{

    private readonly DbAccess _db;
    private readonly IPasswordHasher<User> _hasher;

    public AdminController(DbAccess db, IPasswordHasher<User> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    /// <summary>
    /// Elevates a user to admin
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The user</returns>
    [HttpPost("elevate/admin")]
    public async Task<IActionResult> ElevateToAdmin([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        if (!user.UserRoles.Contains(UserRole.Admin))
        {
            user.UserRoles.Add(UserRole.Admin);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Elevates a user to key user
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The user</returns>
    [HttpPost("elevate/keyUser")]
    public async Task<IActionResult> ElevateToKeyUser([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        if (!user.UserRoles.Contains(UserRole.KeyUser))
        {
            user.UserRoles.Add(UserRole.KeyUser);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Elevates a user to observer
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The user</returns>
    [HttpPost("elevate/observer")]
    public async Task<IActionResult> ElevateToObserver([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        if (!user.UserRoles.Contains(UserRole.Observer))
        {
            user.UserRoles.Add(UserRole.Observer);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Elevates a user to member
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The user</returns>
    [HttpPost("elevate/member")]
    public async Task<IActionResult> ElevateToMember([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        if (!user.UserRoles.Contains(UserRole.Member))
        {
            user.UserRoles.Add(UserRole.Member);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Revokes admin role from user
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The user</returns>
    [HttpDelete("revoke/admin")]
    public async Task<IActionResult> RevokeAdmin([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        if (request.UserId == this.CurrentUser.Id)
        {
            return BadRequest("Du kannst dir nicht selbst den Administratoren-Status wegnehmen");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        if (user.UserRoles.Contains(UserRole.Admin))
        {
            user.UserRoles.Remove(UserRole.Admin);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Revokes key user role from user
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The user</returns>
    [HttpDelete("revoke/keyUser")]
    public async Task<IActionResult> RevokeKeyUser([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        if (user.UserRoles.Contains(UserRole.KeyUser))
        {
            user.UserRoles.Remove(UserRole.KeyUser);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Revokes observer role from user
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The user</returns>
    [HttpDelete("revoke/observer")]
    public async Task<IActionResult> RevokeObserver([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        if (user.UserRoles.Contains(UserRole.Observer))
        {
            user.UserRoles.Remove(UserRole.Observer);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Revokes member role from user
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The user</returns>
    [HttpDelete("revoke/member")]
    public async Task<IActionResult> RevokeMember([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        if (user.UserRoles.Contains(UserRole.Member))
        {
            user.UserRoles.Remove(UserRole.Member);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    /// <summary>
    /// Deletes an user from database
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>Ok result</returns>
    [HttpDelete("deleteUser")]
    public async Task<IActionResult> DeleteUser([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        this._db.EntityManager.Remove(user);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns>All users</returns>
    [HttpGet("allUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        return Ok(await this._db.UserRepository.FindAll());
    }

    /// <summary>
    /// Clocks out a specific user
    /// </summary>
    /// <param name="request">The user request</param>
    /// <returns>Ok result</returns>
    [HttpPost("clockOutUser")]
    public async Task<IActionResult> ClockOutUser([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }
        
        UserStatus? status = await this._db.UserStatusRepository.GetLatestForUser(user);
        if (status == null)
        {
            return BadRequest("Nutzer nicht auf aktiv gesetzt");
        }

        status.ClockOut = DateTime.UtcNow;
        this._db.EntityManager.Update(status);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok(status);
    }

    /// <summary>
    /// Changes the password of a foreign user
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>The updated user</returns>
    [HttpPost("changeForeignPassword")]
    public async Task<IActionResult> UpdateForeignUsersPassword([FromBody] ChangeForeignPasswordRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized("Du bist kein Administrator");
        }

        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("Nutzer nicht gefunden");
        }

        user.Password = this._hasher.HashPassword(user, request.NewPassword);
        this._db.EntityManager.Update(user);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok(user);
    }
}