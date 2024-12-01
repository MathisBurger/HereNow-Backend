using Microsoft.EntityFrameworkCore;
using PresenceBackend.Models.Database;
using PresenceBackend.Shared;

namespace PresenceBackend.Repository;

public class UserStatusRepository : IRepository<UserStatus>
{
    
    private readonly IContext ctx;

    public UserStatusRepository(IContext ctx)
    {
        this.ctx = ctx;
    }
    
    public async Task<UserStatus> FindOneById(Guid id)
    {
        return (await ctx.UserStatuses.FindAsync(id))!;
    }

    public async Task<UserStatus?> GetLatestForUser(User user)
    {
        return await this.ctx.UserStatuses
            .OrderByDescending(e => e.ClockIn)
            .Where(e => e.Owner.Id == user.Id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<UserStatus>> GetAllLoggedIn()
    {
        return await this.ctx.UserStatuses.Where(e => e.ClockOut == null).ToListAsync();
    }
}