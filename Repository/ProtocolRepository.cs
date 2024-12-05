using Microsoft.EntityFrameworkCore;
using PresenceBackend.Models.Database;
using PresenceBackend.Shared;

namespace PresenceBackend.Repository;

/// <summary>
/// Protocol repository
/// </summary>
public class ProtocolRepository : IRepository<Protocol>
{

    private readonly IContext _db;

    public ProtocolRepository(IContext db)
    {
        _db = db;
    }
    
    /// <summary>
    /// Finds one by ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<Protocol?> FindOneById(Guid id)
    {
        return await this._db.Protocols.FindAsync(id);
    }

    /// <summary>
    /// Finds all
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public async Task<List<Protocol>> FindAll(ProtocolAction action)
    {
        return await this._db.Protocols
            .Where(e => e.Action == action)
            .Include(e => e.InvolvedUsers)
            .ToListAsync();
    }
}