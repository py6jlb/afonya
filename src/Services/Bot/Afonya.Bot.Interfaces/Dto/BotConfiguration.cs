namespace Afonya.Bot.Interfaces.Dto;

public record BotConfiguration
{
    public string BotToken { get; init; }
    public string HostAddress { get; init; }
}