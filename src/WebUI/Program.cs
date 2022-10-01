using ExternalConfig;

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

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
