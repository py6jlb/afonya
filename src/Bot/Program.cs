using Bot.Extensions;
using Bot.Services;
using Bot.Services.Dto;
using ExternalConfig;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

var appName = builder.Configuration.GetValue<string>("APP_NAME");
if (string.IsNullOrWhiteSpace(appName))
{
    builder.Configuration.AddApiConfiguration(x =>
    {
        x.AppName = appName;
        x.Optional = false;
        x.Period = int.MaxValue;
        x.ReqUrl = builder.Configuration.GetValue<string>("externalConfigUrl");
    });
}

var sharedConfig = builder.Configuration.GetValue<string>("SHARED_CONFIG");
if (string.IsNullOrWhiteSpace(sharedConfig))
{
    builder.Configuration.AddApiConfiguration(x =>
    {
        x.AppName = sharedConfig;
        x.Optional = false;
        x.Period = int.MaxValue;
        x.ReqUrl = builder.Configuration.GetValue<string>("externalConfigUrl");
    });
}

var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
builder.Services.AddHostedService<ConfigureWebHook>();
builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.BotToken, httpClient));
builder.Services.AddScoped<HandleUpdateService>();
builder.Services.AddControllers().AddNewtonsoftJson();

var app = builder.Build();
app.UseRouting();
app.UseCors();
app.ConfigWithReverseProxy(builder.Configuration);
app.UseEndpoints(endpoints =>
{
    var token = botConfig.BotToken;
    endpoints.MapControllerRoute(
        name: "tgwebhook",
        pattern: $"bot/{token}", 
        new { controller = "WebHook", action = "Post" });
    endpoints.MapControllers();
});

app.Run();
