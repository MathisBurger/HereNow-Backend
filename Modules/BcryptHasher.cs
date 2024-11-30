using Microsoft.AspNetCore.Identity;
using PresenceBackend.Models.Database;

namespace PresenceBackend.Modules;

public class BcryptHasher : IPasswordHasher<User>
{
    public string HashPassword(User user, string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        if (BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword))
        {
            return PasswordVerificationResult.Success;
        }
        return PasswordVerificationResult.Failed;
    }
}