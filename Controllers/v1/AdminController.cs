using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresenceBackend.Filters;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;
using PresenceBackend.Shared;

namespace PresenceBackend.Controllers.v1;

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

    [HttpPost("elevate/admin")]
    public async Task<IActionResult> ElevateToAdmin([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        if (!user.UserRoles.Contains(UserRole.Admin))
        {
            user.UserRoles.Add(UserRole.Admin);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    [HttpPost("elevate/keyUser")]
    public async Task<IActionResult> ElevateToKeyUser([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        if (!user.UserRoles.Contains(UserRole.KeyUser))
        {
            user.UserRoles.Add(UserRole.KeyUser);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    [HttpPost("elevate/observer")]
    public async Task<IActionResult> ElevateToObserver([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        if (!user.UserRoles.Contains(UserRole.Observer))
        {
            user.UserRoles.Add(UserRole.Observer);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    [HttpPost("elevate/member")]
    public async Task<IActionResult> ElevateToMember([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        if (!user.UserRoles.Contains(UserRole.Member))
        {
            user.UserRoles.Add(UserRole.Member);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    [HttpDelete("revoke/admin")]
    public async Task<IActionResult> RevokeAdmin([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        if (user.UserRoles.Contains(UserRole.Admin))
        {
            user.UserRoles.Remove(UserRole.Admin);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    [HttpDelete("revoke/keyUser")]
    public async Task<IActionResult> RevokeKeyUser([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        if (user.UserRoles.Contains(UserRole.KeyUser))
        {
            user.UserRoles.Remove(UserRole.KeyUser);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    [HttpDelete("revoke/observer")]
    public async Task<IActionResult> RevokeObserver([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        if (user.UserRoles.Contains(UserRole.Observer))
        {
            user.UserRoles.Remove(UserRole.Observer);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    [HttpDelete("revoke/member")]
    public async Task<IActionResult> RevokeMember([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        if (user.UserRoles.Contains(UserRole.Member))
        {
            user.UserRoles.Remove(UserRole.Member);
            this._db.EntityManager.Update(user);
            await this._db.EntityManager.SaveChangesAsync();
        }
        return Ok(user);
    }
    
    [HttpDelete("deleteUser")]
    public async Task<IActionResult> DeleteUser([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest();
        }

        this._db.EntityManager.Remove(user);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("allUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }

        return Ok(this._db.UserRepository.FindAll());
    }

    [HttpPost("clockOutUser")]
    public async Task<IActionResult> ClockOutUser([FromBody] UserRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("User does not exist");
        }
        
        UserStatus? status = await this._db.UserStatusRepository.GetLatestForUser(user);
        if (status == null)
        {
            return BadRequest("User not clocked in");
        }

        status.ClockOut = DateTime.UtcNow;
        this._db.EntityManager.Update(status);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok(status);
    }

    [HttpPost("changeForeignPassword")]
    public async Task<IActionResult> UpdateForeignUsersPassword([FromBody] ChangeForeignPasswordRequest request)
    {
        if (this.CurrentUser == null || !this.CurrentUser.UserRoles.Contains(UserRole.Admin))
        {
            return Unauthorized();
        }
        
        User? user = await this._db.UserRepository.FindOneById(request.UserId);
        if (user == null)
        {
            return BadRequest("User does not exist");
        }
        
        user.Password = this._hasher.HashPassword(user, request.NewPassword);
        this._db.EntityManager.Update(user);
        await this._db.EntityManager.SaveChangesAsync();
        return Ok(user);
    }
}