using Microsoft.AspNetCore.HttpOverrides;

namespace Bot.Extensions;

public static class ReverseProxyExtensions
{
    public static WebApplication ConfigWithReverseProxy(this WebApplication app, IConfiguration config)
    {
        _ = bool.TryParse(config["USE_REVERSE_PROXY"], out var behindReverseProxy);
        if (!behindReverseProxy) return app;
        
        var forwardedHeaderOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        };
        forwardedHeaderOptions.KnownNetworks.Clear();
        forwardedHeaderOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(forwardedHeaderOptions);
        var subDirPath = config["SUBDIR_PATH"];
        
        if (!string.IsNullOrWhiteSpace(subDirPath)) app.UsePathBase(new PathString(subDirPath));

        return app;
    }
}
