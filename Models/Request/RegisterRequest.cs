using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

public class RegisterRequest
{
    [JsonPropertyName("username")]
    public string Username { get; set; }
    
    [JsonPropertyName("password")]
    public string Password { get; set; }
}