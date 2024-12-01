using System.Text.Json.Serialization;

namespace PresenceBackend.Models.Request;

public class ElevateRequest
{
    [JsonPropertyName("userId")] 
    public Guid UserId { get; set; }
}