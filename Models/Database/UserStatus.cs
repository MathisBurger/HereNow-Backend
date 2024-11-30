using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresenceBackend.Models.Database;

public class UserStatus
{
    [Key]
    public Guid Id { get; set; }
    
    public DateTime? ClockIn { get; set; }

    public DateTime? ClockOut { get; set; } = null;
    
    [System.Text.Json.Serialization.JsonIgnore]
    [InverseProperty("UserStatuses")]
    public User? Owner { get; set; }
}