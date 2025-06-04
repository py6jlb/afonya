using System;
using System.Text.Json.Serialization;

namespace Shared.Contracts;

public class AuthenticateResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
    public bool IsAdmin { get; set; }

    // [JsonConstructor]
    // public AuthenticateResponse(string? id, string login, string token)
    // {
    //     Id = id;
    //     Name = login;
    //     Token = token;
    // }

    [JsonConstructor]
    public AuthenticateResponse()
    {
    }

    public AuthenticateResponse(UserDto user, string token)
    {
        Id = user.Id;
        Name = user.Login;
        IsAdmin = user.IsAdmin;
        Token = token;
    }
}
