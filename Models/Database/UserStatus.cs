using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresenceBackend.Models.Database;

/// <summary>
/// User status model
/// </summary>
public class UserStatus
{
    /// <summary>
    /// The ID of the status
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Clock in timestamp
    /// </summary>
    public DateTime? ClockIn { get; set; }

    /// <summary>
    /// Clock out timestamp
    /// </summary>
    public DateTime? ClockOut { get; set; } = null;
    
    /// <summary>
    /// The owner of the status
    /// </summary>
    [System.Text.Json.Serialization.JsonIgnore]
    [InverseProperty("UserStatuses")]
    public User? Owner { get; set; }
}