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

    public AdminController(DbAccess db)
    {
        _db = db;
    }

    [HttpPost("elevate/admin")]
    public async Task<IActionResult> ElevateToAdmin([FromBody] ElevateRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.UserRoles.Contains(UserRole.Admin))
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
    public async Task<IActionResult> ElevateToKeyUser([FromBody] ElevateRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.UserRoles.Contains(UserRole.Admin))
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
    public async Task<IActionResult> ElevateToObserver([FromBody] ElevateRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.UserRoles.Contains(UserRole.Admin))
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
    public async Task<IActionResult> ElevateToMember([FromBody] ElevateRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.UserRoles.Contains(UserRole.Admin))
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
    
    [HttpPost("revoke/admin")]
    public async Task<IActionResult> RevokeAdmin([FromBody] ElevateRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.UserRoles.Contains(UserRole.Admin))
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
    
    [HttpPost("revoke/keyUser")]
    public async Task<IActionResult> RevokeKeyUser([FromBody] ElevateRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.UserRoles.Contains(UserRole.Admin))
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
    
    [HttpPost("revoke/observer")]
    public async Task<IActionResult> RevokeObserver([FromBody] ElevateRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.UserRoles.Contains(UserRole.Admin))
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
    
    [HttpPost("revoke/member")]
    public async Task<IActionResult> RevokeMember([FromBody] ElevateRequest request)
    {
        if (this.CurrentUser == null || this.CurrentUser.UserRoles.Contains(UserRole.Admin))
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
}