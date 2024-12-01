using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

public class UserRequest
{
    [JsonPropertyName("userId")] 
    public Guid UserId { get; set; }
}