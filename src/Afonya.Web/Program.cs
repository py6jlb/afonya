using Afonya.Web.Extensions;
using Afonya.Web.Middleware;
using Common.Extensions;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwaggerGeneration();
builder.AddErrorHandling(builder.Environment);
builder.AddServices(builder.Configuration);
builder.AddBotServices(builder.Configuration);
builder.Host.UseSerilog();

var app = builder.Build();
if (app.Environment.IsDevelopment()) app.UseSwaggerUi(builder);

app.UseRouting();
if (app.Environment.IsDevelopment()) app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseAuthorization();
app.UseProblemDetails();
app.UseReverseProxy();

app.UseMiddleware<JwtMiddleware>();
await app.InitUsers();
app.MapControllers();
app.MapBotController();
app.Run();