using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresenceBackend.Models.Database;

public class User
{
    [Key]
    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Username { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public string Password { get; set; }
    
    [System.Text.Json.Serialization.JsonIgnore]
    public string? RefreshToken { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public DateTime? RefreshTokenExpiry { get; set; }
    
    public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public string Email { get; set; } = "demo@rathje-vt.de";

    [InverseProperty("Owner")] 
    public IList<UserStatus> UserStatuses { get; set; } = new List<UserStatus>();

    [InverseProperty("InvolvedUsers")]
    public IList<Protocol> InvolvedIn { get; set; } = new List<Protocol>();
    
    [InverseProperty("Creator")]
    public IList<Protocol> CreatedProtocols { get; set; } = new List<Protocol>(); 
}