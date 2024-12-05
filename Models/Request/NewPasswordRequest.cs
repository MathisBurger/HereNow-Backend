using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

/// <summary>
/// Request to set a new password
/// </summary>
public class NewPasswordRequest
{
    /// <summary>
    /// The old password
    /// </summary>
    [JsonPropertyName("oldPassword")]
    public string OldPassword { get; set; }
    
    /// <summary>
    /// The new password
    /// </summary>
    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; }
}