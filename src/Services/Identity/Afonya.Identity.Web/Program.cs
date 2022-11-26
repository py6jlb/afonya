using Afonya.Identity.Domain;
using Afonya.Identity.Infrastructure;
using Afonya.Identity.Logic.Services;
using Afonya.Identity.Web.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<DataSeedService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var identityConfig = builder.Configuration.GetSection("IdentityConfig").Get<Config>();
var identityBuilder = builder.Services.AddIdentityServer(options =>
    {
        options.Events.RaiseErrorEvents = true;
        options.Events.RaiseInformationEvents = true;
        options.Events.RaiseFailureEvents = true;
        options.Events.RaiseSuccessEvents = true;
    })
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryApiResources(identityConfig.GetApis())
    .AddInMemoryClients(identityConfig.GetClients())
    .AddInMemoryApiScopes(identityConfig.GetApiScopes())
    .AddAspNetIdentity<ApplicationUser>();

identityBuilder.AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.UseIdentityServer();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();