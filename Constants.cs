namespace PresenceBackend;

public class Constants
{
    /// <summary>
    /// Session duration with access token
    /// </summary>
    public static readonly TimeSpan AccessTokenSessionDuration = TimeSpan.FromMinutes(15);
    
    /// <summary>
    /// The refresh token session duration
    /// </summary>
    public static readonly TimeSpan RefreshTokenSessionDuration = TimeSpan.FromDays(30);
}