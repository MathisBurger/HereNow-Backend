using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using JWT.Algorithms;
using JWT.Builder;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;
using PresenceBackend.Shared;

namespace PresenceBackend.Services;

/// <summary>
/// Custom auth implementation
/// </summary>
public class CustomAuthorization : IAuthorization
{

    private readonly DbAccess Db;
    private readonly JwtBuilder JwtBuilder;

    public CustomAuthorization(DbAccess db, IConfiguration configuration)
    {
        this.Db = db;
        var jwtSignKey = Encoding.UTF8.GetBytes(configuration.GetValue<string>("jwtKey")!);
        this.JwtBuilder = new JwtBuilder().WithSecret(jwtSignKey).WithAlgorithm(new HMACSHA256Algorithm());
    }
    
    public string GenerateAccessToken(User claims) => 
        JwtBuilder
            .AddClaim("uid", claims.Id)
            .ExpirationTime(DateTime.UtcNow.Add(Constants.AccessTokenSessionDuration))
            .IssuedAt(DateTime.UtcNow)
            .Encode();
    

    public async Task<string> GenerateRefreshToken(User claims)
    {
        claims.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(256));
        claims.RefreshTokenExpiry = DateTime.UtcNow.Add(Constants.RefreshTokenSessionDuration);
        this.Db.EntityManager.Update(claims);
        await this.Db.EntityManager.SaveChangesAsync();
        return claims.RefreshToken!;
    }

    public async Task<User?> ValidateRefreshToken(string token)
    {
        User? user = await this.Db.UserRepository.FindByRefreshToken(token);
        if (user != null && user.RefreshTokenExpiry > DateTime.UtcNow)
        {
            return user;
        }
        return null;
    }

    public async Task<User?> ValidateAccessToken(string token)
    {
        IDictionary<string, object> claims = JwtBuilder.MustVerifySignature().Decode<IDictionary<string, object>>(token);
        if (!claims.ContainsKey("uid"))
        {
            return null;
        }

        if (!claims.TryGetValue("uid", out var userId))
        {
            return null;
        }

        string? stringUid = userId?.ToString();
        if (Guid.TryParse(stringUid, out Guid uid))
        {
            return await this.Db.UserRepository.FindOneById(uid);
        }

        return null;
    }

    public async Task<bool> ValidateLoginCredentials(LoginRequest loginRequest)
    {
        return await this.Db.UserRepository.LoginUser(loginRequest);
    }
}