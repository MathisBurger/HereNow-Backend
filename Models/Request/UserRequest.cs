using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

/// <summary>
/// Generic user request
/// </summary>
public class UserRequest
{
    
    /// <summary>
    /// The ID of the user
    /// </summary>
    [JsonPropertyName("userId")] 
    public Guid UserId { get; set; }
}