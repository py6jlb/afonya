using Afonya.Web.Extensions;
using Common.Extensions;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.AddErrorHandling(builder.Environment);
builder.AddServices(builder.Configuration);
builder.AddBotServices(builder.Configuration);
builder.Host.UseSerilog();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseProblemDetails();
app.UseReverseProxy();

await app.InitUsers();

app.MapRazorPages().RequireAuthorization();
app.Run();