using Bot.Extensions;
using Bot.Services;
using Bot.Services.Dto;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
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
