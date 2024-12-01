using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresenceBackend.Models.Database;

public class Protocol
{
    [Key]
    public Guid Id { get; set; }
    
    [InverseProperty("CreatedProtocols")]
    public required User Creator { get; set; }
    
    public required ProtocolAction Action { get; set; }
    
    [InverseProperty("InvolvedIn")] 
    public IList<User> InvolvedUsers { get; set; } = new List<User>();
    
    public DateTime Timestamp { get; } = DateTime.Now;
}