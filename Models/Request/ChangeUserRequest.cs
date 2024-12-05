using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

public class ChangeUserRequest
{
    
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }
    
    [JsonPropertyName("email")]
    public string Email { get; set; }
}