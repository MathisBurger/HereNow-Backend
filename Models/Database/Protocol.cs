using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PresenceBackend.Models.Database;

/// <summary>
/// Protocol model
/// </summary>
public class Protocol
{
    /// <summary>
    /// The ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// The creator of protocol
    /// </summary>
    public required User? Creator { get; set; }
    
    /// <summary>
    /// The action of the protocol
    /// </summary>
    public required ProtocolAction Action { get; set; }
    
    /// <summary>
    /// All involved users
    /// </summary>
    [InverseProperty("InvolvedIn")] 
    public IList<User> InvolvedUsers { get; set; } = new List<User>();
    
    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; } = DateTime.Now;
}