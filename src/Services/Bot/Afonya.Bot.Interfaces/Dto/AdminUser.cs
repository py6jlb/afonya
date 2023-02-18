namespace Afonya.Bot.Interfaces.Dto;

public record AdminUser
{
    public string Username { get; init; }
    public string Password { get; init; }
};