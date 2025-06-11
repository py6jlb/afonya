using System;

namespace Afonya.Api.Interfaces.Services;

public interface IHashService
{
    string HashPassword(string password);
    bool VerifyPassword(string hash, string password);
}
