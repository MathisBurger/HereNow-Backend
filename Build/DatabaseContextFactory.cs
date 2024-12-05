using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PresenceBackend.Shared;

namespace PresenceBackend.Build;

/// <summary>
/// Factory to build database context for development
/// </summary>
public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // Ensure the right directory is used
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<DatabaseContext>();
        var connectionString = configuration.GetConnectionString("MyDatabaseConnection");
        optionsBuilder.UseSqlServer(connectionString);

        return new DatabaseContext(configuration);
    }
}