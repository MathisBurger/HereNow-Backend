namespace PresenceBackend.Models.Request;

/// <summary>
/// Request for obtaining a token
/// </summary>
/// <param name="token">The previous token</param>
public class TokenRequest(string token)
{
    /// <summary>
    /// The token
    /// </summary>
    public string Token { get; } = token;
}