namespace Afonya.Api.Logic.Services;

using Afonya.Api.Interfaces.Services;
using BCrypt.Net;

public class HashService : IHashService
{
    public string HashPassword(string password)
    {
        string passwordHash = BCrypt.HashPassword(password);
        return passwordHash;
    }

    public bool VerifyPassword(string hash, string password)
    {
        var result = BCrypt.Verify(password, hash);
        return result;
    }
}
