using Identity.Web.Config;

var builder = WebApplication.CreateBuilder(args);
var identityConfig = builder.Configuration.GetSection("IdentityConfig").Get<Config>();
var identityBuilder = builder.Services.AddIdentityServer()
    .AddInMemoryApiScopes(identityConfig.GetApiScopes())
    .AddInMemoryClients(identityConfig.GetClients());
identityBuilder.AddDeveloperSigningCredential();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseIdentityServer();
app.Run();