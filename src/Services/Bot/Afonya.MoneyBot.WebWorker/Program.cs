using Afonya.MoneyBot.WebWorker.Extensions;
using Common.Extensions;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGeneration();
builder.Services.AddErrorHandling(builder.Environment);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddBotServices();
builder.Services.AddControllers();
builder.Host.UseSerilog();

var app = builder.Build();
if(app.Environment.IsDevelopment()) app.UseSwaggerUi(builder);
app.UseRouting();
app.UseProblemDetails();
app.UseReverseProxy();
app.MapControllers();
app.MapBotController();
app.Run();