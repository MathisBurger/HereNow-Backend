using Microsoft.EntityFrameworkCore;
using PresenceBackend.Models.Database;

namespace PresenceBackend.Shared;

public class DatabaseContext : DbContext, IContext
{

    private readonly IConfiguration configuration;
    
    public DbSet<User> Users { get; set; }

    public DatabaseContext(IConfiguration configuration)
    {
        this.configuration = configuration;
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        
        
        base.OnConfiguring(optionsBuilder);
    }
}