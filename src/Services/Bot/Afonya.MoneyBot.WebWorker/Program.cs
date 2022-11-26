using Afonya.MoneyBot.Interfaces.Dto;
using Afonya.MoneyBot.WebWorker.Extensions;
using Common.Extensions;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);
var botConfig = builder.Configuration.GetSection("BotConfiguration").Get<BotConfiguration>();
builder.Services.AddSwaggerGeneration();
builder.Services.AddErrorHandling(builder.Environment);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddBotServices(botConfig);
builder.Services.AddControllers();
builder.Host.UseSerilog();

var app = builder.Build();
if(app.Environment.IsDevelopment()) app.UseSwaggerUi(builder);
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