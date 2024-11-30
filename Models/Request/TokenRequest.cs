namespace PresenceBackend.Models.Request;

public class TokenRequest(string token)
{
    public string Token { get; } = token;
}