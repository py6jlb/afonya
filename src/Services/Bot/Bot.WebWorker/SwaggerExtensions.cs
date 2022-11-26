using Microsoft.OpenApi.Models;

namespace Bot.Host;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerGeneration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Хранение данных бота", Version = "v1" });
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app, WebApplicationBuilder builder)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "Бот store V1");
        });
        return app;
    }
}
