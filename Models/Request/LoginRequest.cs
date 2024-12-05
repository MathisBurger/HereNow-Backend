using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

/// <summary>
/// Request to log in user
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Username
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; }
    
    /// <summary>
    /// Password
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; }
}