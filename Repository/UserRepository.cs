using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;
using PresenceBackend.Shared;

namespace PresenceBackend.Repository;

/// <summary>
/// User repository
/// </summary>
public class UserRepository : IRepository<User>
{
    private readonly IContext ctx;
    private readonly IPasswordHasher<User> hasher;

    public UserRepository(IContext ctx, IPasswordHasher<User> hasher)
    {
        this.ctx = ctx;
        this.hasher = hasher;
    }
    
    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<User> RegisterUser(RegisterRequest request)
    {
        var user = new User();
        user.Username = request.Username;
        user.Password = hasher.HashPassword(user, request.Password);
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.UserRoles = new List<UserRole>() { UserRole.Member };
        ctx.Add(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    /// <summary>
    /// Logs in a user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> LoginUser(LoginRequest request)
    {
        var user = await FindUserByUsername(request.Username);
        if (user == null)
        {
            return false;
        }

        return hasher.VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Success;
    }

    /// <summary>
    /// Finds user by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public async Task<User?> FindUserByUsername(string username)
    {
        return await ctx.Users
            .Where(u => u.Username == username)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Finds user by refresh token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<User?> FindByRefreshToken(string token)
    {
        return await ctx.Users
            .Where(u => u.RefreshToken == token)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Finds user by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<User?> FindOneById(Guid id)
    {
        return await ctx.Users.FindAsync(id);
    }

    /// <summary>
    /// Finds current clocked in user
    /// </summary>
    /// <returns></returns>
    public async Task<List<User>> FindCurrentClockedIn()
    {
        return await ctx.Users
            .GroupJoin(
                ctx.UserStatuses,
                user => user,
                status => status.Owner,
                (user, userStatusGroup) => new {user, userStatusGroup}
            )
            .SelectMany(
                x => x.userStatusGroup.DefaultIfEmpty(),
                (x, status) => new { x.user, status }
            )
            .Where(x => x.status != null && x.status.ClockOut == null)
            .Select(x => x.user)
            .Include(x => x.UserStatuses.First())
            .ToListAsync();
    }

    /// <summary>
    /// Finds all users
    /// </summary>
    /// <returns></returns>
    public async Task<List<User>> FindAll()
    {
        return await this.ctx.Users.ToListAsync();
    }
}