using Microsoft.Extensions.Hosting;
using Serilog;

namespace Common.Extensions;

public static class SerilogExtensions
{
    public static IHostBuilder UseSerilog(this IHostBuilder hostBuilder)
    {

        hostBuilder.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
            .ReadFrom.Configuration(hostingContext.Configuration)
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
        );

        return hostBuilder;
    }

}