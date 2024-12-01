using Microsoft.EntityFrameworkCore;
using PresenceBackend.Models.Database;

namespace PresenceBackend.Shared;

public class DatabaseContext : DbContext, IContext
{

    private readonly IConfiguration configuration;
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserStatus> UserStatuses { get; set; }
    public DbSet<Protocol> Protocols { get; set; }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<User>()
            .HasIndex(e => e.Username)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(e => e.RefreshToken)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(e => e.Email)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .HasMany<Protocol>(e => e.CreatedProtocols)
            .WithOne(e => e.Creator)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Protocol>()
            .HasMany<User>(e => e.InvolvedUsers)
            .WithMany(u => u.InvolvedIn)
            .UsingEntity<Dictionary<string, object>>(
                "ProtocolUsers",
                j => j.HasOne<User>().WithMany().OnDelete(DeleteBehavior.NoAction),
                j => j.HasOne<Protocol>().WithMany().OnDelete(DeleteBehavior.Cascade)
            );
        
        modelBuilder.Entity<UserStatus>()
            .HasOne<User>(e => e.Owner)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }
}