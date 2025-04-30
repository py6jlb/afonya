using System;

namespace Shared.Contracts;

public class AuthenticateResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }


    public AuthenticateResponse(UserDto user, string token)
    {
        Id = user.Id;
        Name = user.Login;
        Token = token;
    }
}
