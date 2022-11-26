using Common.Extensions;
using Hellang.Middleware.ProblemDetails;
using MoneyBot.Interfaces.Dto;
using MoneyBot.WebWorker.Extensions;

var builder = WebApplication.CreateBuilder(args);
var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
builder.Services.AddSwaggerGeneration();
builder.Services.AddErrorHandling(builder.Environment);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddBotServices(botConfig);
builder.Services.AddControllers();
builder.Host.UseSerilog();

var app = builder.Build();
app.UseSwaggerUi(builder);
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseProblemDetails();
app.UseReverseProxy(builder.Configuration);
app.UseEndpoints(endpoints =>
{
    endpoints.MapBotController(botConfig);
    endpoints.MapControllers();
});
app.Run();