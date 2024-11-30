namespace PresenceBackend.Models.Response;

public class TokenResponse(string token, string tokenType)
{
    public string Token { get; } = token;
    public string TokenType { get; } = tokenType;
    public DateTime IssuedAt { get; } = DateTime.UtcNow;
}