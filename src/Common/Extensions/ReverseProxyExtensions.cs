﻿using Common.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Extensions;

public static class ReverseProxyExtensions
{
    /// <summary>
    /// Добавить реверсивный прокси
    /// </summary>
    /// <param name="app">приложение</param>
    public static WebApplication UseReverseProxy(this WebApplication app)
    {
        var opt = app.Services.GetRequiredService<IOptions<ReverseProxyConfig>>().Value;
        var logger = app.Services.GetRequiredService<ILogger<WebApplication>>();
        
        var useProxy = opt?.UseReverseProxy ?? false;

        if (!useProxy)
        {
            logger.LogInformation($"Reverse-прокси не используется");
            return app;
        }
        
        var subDirPath = opt?.SubDir ?? "";
        if (!string.IsNullOrWhiteSpace(subDirPath))
        {
            logger.LogInformation("Базовый путь: {path}", subDirPath);
            app.UsePathBase(new PathString(subDirPath));
        }

        var forwardedHeaderOptions = new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        };
        forwardedHeaderOptions.KnownNetworks.Clear();
        forwardedHeaderOptions.KnownProxies.Clear();
        app.UseForwardedHeaders(forwardedHeaderOptions);

        return app;
    }

}
