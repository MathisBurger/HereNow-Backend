using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

/// <summary>
/// Request to change user
/// </summary>
public class ChangeUserRequest
{
 
    /// <summary>
    /// First name of the user
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    
    /// <summary>
    /// Last name of the user
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    
    /// <summary>
    /// The email of the user
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
}