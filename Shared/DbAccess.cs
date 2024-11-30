using Microsoft.AspNetCore.Identity;
using PresenceBackend.Models.Database;
using PresenceBackend.Repository;

namespace PresenceBackend.Shared;

public class DbAccess
{
    public readonly IContext EntityManager;
    public readonly UserRepository UserRepository;

    public DbAccess(IContext context, IPasswordHasher<User> hasher)
    {
        EntityManager = context;
        UserRepository = new UserRepository(context, hasher);
        
    }
}