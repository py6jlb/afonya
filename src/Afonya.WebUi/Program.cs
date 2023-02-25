using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Afonya.WebUi;
using Afonya.WebUi.HttpClients;
using Afonya.WebUi.HttpClients.Abstractions;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient<IManageService, ManageService>()
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth", options.ProviderOptions);
});

await builder.Build().RunAsync();