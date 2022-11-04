using Bot.Host;
using Bot.Infrastructure.Contexts;
using Bot.Interfaces;
using Bot.Interfaces.Dto;
using Bot.Interfaces.Services;
using Bot.Logic.Services;
using Common.Extensions;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();

//Swagger
builder.Services.AddSwaggerGeneration();

//DI
builder.Services.AddHostedService<ConfigureWebHook>();
builder.Services.AddHostedService<DataSeedService>();

builder.Services.AddSingleton<ILiteDbContext>(p => 
    new DbContext(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddHttpClient("tgwebhook")
    .AddTypedClient<ITelegramBotClient>(httpClient => new TelegramBotClient(botConfig.BotToken, httpClient));
builder.Services.AddScoped<IHandleUpdateService, HandleUpdateService>();
builder.Services.AddTransient<IMoneyTransactionService, MoneyTransactionService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddControllers();

var app = builder.Build();
app.UseSwaggerUI(builder);
app.UseRouting();
app.ConfigWithReverseProxy(builder.Configuration);
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "tgwebhook",
        pattern: $"bot/{botConfig.BotToken}", 
        new { controller = "WebHook", action = "Post" });
    endpoints.MapControllers();
});
app.Run();