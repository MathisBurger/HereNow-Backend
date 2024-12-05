namespace PresenceBackend.Models.Response;

/// <summary>
/// Token response
/// </summary>
/// <param name="token"></param>
/// <param name="tokenType"></param>
public class TokenResponse(string token, string tokenType)
{
    /// <summary>
    /// The token
    /// </summary>
    public string Token { get; } = token;
    
    /// <summary>
    /// The type of the token
    /// </summary>
    public string TokenType { get; } = tokenType;
    
    /// <summary>
    /// The issue date of the token
    /// </summary>
    public DateTime IssuedAt { get; } = DateTime.UtcNow;
}