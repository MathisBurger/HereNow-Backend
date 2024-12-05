using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresenceBackend.Models.Database;

/// <summary>
/// User model
/// </summary>
public class User
{
    /// <summary>
    /// ID of the user
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The first name of the user
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// The last name of the user
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// The username of the user
    /// </summary>
    public string Username { get; set; }
    
    /// <summary>
    /// The password of the user
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string Password { get; set; }
    
    /// <summary>
    /// The refresh token of the user
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public string? RefreshToken { get; set; }
    
    /// <summary>
    /// Token expire date
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    public DateTime? RefreshTokenExpiry { get; set; }
    
    /// <summary>
    /// All roles of the user
    /// </summary>
    public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// The email opf the user
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// All states of the user
    /// </summary>
    [InverseProperty("Owner")] 
    public IList<UserStatus> UserStatuses { get; set; } = new List<UserStatus>();

    /// <summary>
    /// All involvements of the user
    /// </summary>
    [InverseProperty("InvolvedUsers")]
    public IList<Protocol> InvolvedIn { get; set; } = new List<Protocol>();
    
    /// <summary>
    /// All protocols the user has created
    /// </summary>
    public IList<Protocol> CreatedProtocols { get; set; } = new List<Protocol>(); 
}