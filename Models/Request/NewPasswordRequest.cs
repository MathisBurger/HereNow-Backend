using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

public class NewPasswordRequest
{
    [JsonPropertyName("oldPassword")]
    public string OldPassword { get; set; }
    
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
}