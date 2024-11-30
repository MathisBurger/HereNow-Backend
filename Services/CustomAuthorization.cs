using System.Collections.Immutable;
using System.Security.Cryptography;
using System.Text;
using JWT.Algorithms;
using JWT.Builder;
using PresenceBackend.Models.Database;
using PresenceBackend.Models.Request;
using PresenceBackend.Shared;

namespace PresenceBackend.Services;

public class CustomAuthorization : IAuthorization
{
    private readonly byte[] jwtSignKey;

    private readonly DbAccess Db;

    private JwtBuilder JwtBuilder
    {
        get => new JwtBuilder().WithSecret(jwtSignKey).WithAlgorithm(new HMACSHA256Algorithm());
    }

    public CustomAuthorization(DbAccess db)
    {
        this.Db = db;
        this.jwtSignKey = RandomNumberGenerator.GetBytes(2048);
    }

    public string GenerateAccessToken(User claims) => 
        JwtBuilder
            .AddClaim("uid", claims.Id)
            .ExpirationTime(DateTime.UtcNow.Add(Constants.AccessTokenSessionDuration))
            .IssuedAt(DateTime.UtcNow)
            .Encode();
    

    public string GenerateRefreshToken(User claims)
    {
        claims.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(256));
        claims.RefreshTokenExpiry = DateTime.UtcNow.Add(Constants.RefreshTokenSessionDuration);
        this.Db.EntityManager.Update(claims);
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
        IDictionary<string, string> claims = JwtBuilder.MustVerifySignature().Decode<IDictionary<string, string>>(token);
        if (!claims.ContainsKey("uid"))
        {
            return null;
        }
        Guid uid = Guid.Parse(claims["uid"]);
        return await this.Db.UserRepository.FindOneById(uid);
    }

    public async Task<bool> ValidateLoginCredentials(LoginRequest loginRequest)
    {
        return await this.Db.UserRepository.LoginUser(loginRequest);
    }
}