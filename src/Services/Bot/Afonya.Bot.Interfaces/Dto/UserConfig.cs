namespace Afonya.Bot.Interfaces.Dto;

public record UserConfig
{
    public string Username { get; init; }
    public string Password { get; init; }
    public bool IsAdmin { get; init; }
};