using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

/// <summary>
/// Request to register a new user
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// The username
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; }
    
    /// <summary>
    /// The password
    /// </summary>
    [JsonPropertyName("password")]
    public string Password { get; set; }
    
    /// <summary>
    /// The first name
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    
    /// <summary>
    /// The last name
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    
    /// <summary>
    /// The email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
}