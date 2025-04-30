using Afonya.Bot.WebWorker.Extensions;
using Afonya.Bot.WebWorker.Middleware;
using Common.Extensions;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwaggerGeneration();
builder.AddErrorHandling(builder.Environment);
builder.AddServices(builder.Configuration);
builder.AddBotServices(builder.Configuration);
builder.Host.UseSerilog();

var app = builder.Build();
if(app.Environment.IsDevelopment()) app.UseSwaggerUi(builder);

app.UseMiddleware<JwtMiddleware>();
app.UseRouting();
app.UseProblemDetails();
app.UseReverseProxy();
app.MapControllers();
app.MapBotController();
app.Run();