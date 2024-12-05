using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;

namespace PresenceBackend.Services;

/// <summary>
/// Auth interface
/// </summary>
public interface IAuthorization
{

    /// <summary>
    /// Generates an access token
    /// </summary>
    /// <param name="claims">The user claims</param>
    /// <returns>The access token</returns>
    string GenerateAccessToken(User claims);
    
    /// <summary>
    /// Generates an refresh token
    /// </summary>
    /// <param name="claims">The user claims</param>
    /// <returns>The refresh token</returns>
    Task<string> GenerateRefreshToken(User claims);
    
    /// <summary>
    /// Validates an refresh token
    /// </summary>
    /// <param name="token">The token</param>
    /// <returns>The user</returns>
    Task<User?> ValidateRefreshToken(string token);
    
    /// <summary>
    /// Validates an access token
    /// </summary>
    /// <param name="token">The access token</param>
    /// <returns>The user</returns>
    Task<User?> ValidateAccessToken(string token);
    
    /// <summary>
    /// Validates login credentials
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    Task<bool> ValidateLoginCredentials(LoginRequest loginRequest);

}