using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

public class NewPasswordRequest
{
    [JsonPropertyName("old-password")]
    public string OldPassword { get; set; }
    
    [JsonPropertyName("new-password")]
    public string NewPassword { get; set; }
}