using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MoneyBot.WebWorker.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerGeneration(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "JWT Аутентификация",
                Description = "Вводите **_только_** JWT Bearer токен.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // must be lower case
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {securityScheme, Array.Empty<string>()}
            });

            c.SupportNonNullableReferenceTypes();
            c.UseDateOnlyTimeOnlyStringConverters();

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Хранение данных бота", Version = "v1" });
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerUi(this IApplicationBuilder app, WebApplicationBuilder builder)
    {
        app.UseSwagger(c => 
        {
            c.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
            {
                var schema = httpRequest.Scheme;
                var host = httpRequest.Headers["Host"];
                var subDir = builder.Configuration["SUBDIR"] ?? "";
                var serverUrl = $"{schema}://{host}{subDir}";
                swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
            });
        });
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("v1/swagger.json", "Бот store V1");
        });
        return app;
    }
    
    private static void UseDateOnlyTimeOnlyStringConverters(this SwaggerGenOptions options)
    {
        options.MapType<DateOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "date",
            Example = OpenApiAnyFactory.CreateFromJson("\"2022-02-23\""),
            Description = "Формат для указания только даты, год-месяц-день"
        });
        options.MapType<TimeOnly>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "time",
            Example = OpenApiAnyFactory.CreateFromJson("\"0:11:42\""),
            Description = "Формат для указания только времени, часы:минуты:секунды"
        });
        options.MapType<TimeSpan>(() => new OpenApiSchema
        {
            Type = "string",
            Format = "timeSpan",
            Example = OpenApiAnyFactory.CreateFromJson("\"1.4:25:00\""),
            Description = "Формат для указания интервала, дни.часы:минуты:секунды"
        });
    }

}
