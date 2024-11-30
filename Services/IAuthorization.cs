using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;

namespace PresenceBackend.Services;

public interface IAuthorization
{

    string GenerateAccessToken(User claims);
    
    string GenerateRefreshToken(User claims);
    
    Task<User?> ValidateRefreshToken(string token);
    
    Task<User?> ValidateAccessToken(string token);
    
    Task<bool> ValidateLoginCredentials(LoginRequest loginRequest);

}