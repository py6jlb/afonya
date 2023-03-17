namespace Common.Options;

public record ReverseProxyConfig
{
    public bool UseReverseProxy { get; init; }
    public string? SubDir { get; init; }
}