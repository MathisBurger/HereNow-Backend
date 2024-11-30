namespace PresenceBackend;

public class Constants
{
    public static readonly TimeSpan AccessTokenSessionDuration = TimeSpan.FromMinutes(15);
    
    public static readonly TimeSpan RefreshTokenSessionDuration = TimeSpan.FromDays(30);
}