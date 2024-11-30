using System.ComponentModel.DataAnnotations;

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
    
}