namespace Afonya.Bot.Logic.Services;
using BCrypt.Net;

public static class HashService
{
    public static string HashPassword(string password)
    {
        string passwordHash = BCrypt.HashPassword(password);
        return passwordHash;
    }

    public static bool VerifyPassword(string hash, string password)
    {
        var result = BCrypt.Verify(password, hash);
        return result;
    }
}
