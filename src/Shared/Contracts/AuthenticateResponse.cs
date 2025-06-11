using System;
using System.Text.Json.Serialization;

namespace Shared.Contracts;

public class AuthenticateResponse
{
    public string Id { get; set; }
    public string Name { get; set; }
    public bool IsAdmin { get; set; }

    [JsonConstructor]
    public AuthenticateResponse()
    {
    }

    public AuthenticateResponse(UserDto user)
    {
        Id = user.Id;
        Name = user.Login;
        IsAdmin = user.IsAdmin;
    }
}
