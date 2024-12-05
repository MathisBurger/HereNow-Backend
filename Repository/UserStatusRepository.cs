using Microsoft.EntityFrameworkCore;
using PresenceBackend.Models.Database;
using PresenceBackend.Shared;

namespace PresenceBackend.Repository;

/// <summary>
/// User status repository
/// </summary>
public class UserStatusRepository : IRepository<UserStatus>
{
    
    private readonly IContext ctx;

    public UserStatusRepository(IContext ctx)
    {
        this.ctx = ctx;
    }
    
    /// <summary>
    /// Finds one by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserStatus> FindOneById(Guid id)
    {
        return (await ctx.UserStatuses.FindAsync(id))!;
    }

    /// <summary>
    /// Gets the latest for specific user
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public async Task<UserStatus?> GetLatestForUser(User user)
    {
        return await this.ctx.UserStatuses
            .OrderByDescending(e => e.ClockIn)
            .Where(e => e.Owner.Id == user.Id)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Gets all logged-in sessions
    /// </summary>
    /// <returns></returns>
    public async Task<List<UserStatus>> GetAllLoggedIn()
    {
        return await this.ctx.UserStatuses.Where(e => e.ClockOut == null).ToListAsync();
    }
}