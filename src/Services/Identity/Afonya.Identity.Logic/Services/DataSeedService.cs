using System.Security.Claims;
using Afonya.Identity.Domain;
using Afonya.Identity.Infrastructure;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Afonya.Identity.Logic.Services;

public class DataSeedService : IHostedService
{
    private readonly ILogger<DataSeedService> _logger;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;

    public DataSeedService(
        ILogger<DataSeedService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _services = serviceProvider;
        _configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _services.CreateScope();
        var context = _services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();

        var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var alice = userMgr.FindByNameAsync("alice").Result;
        if (alice == null)
        {
            alice = new ApplicationUser
            {
                UserName = "alice"
            };
            var result = userMgr.CreateAsync(alice, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(alice, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Alice Smith"),
                new Claim(JwtClaimTypes.GivenName, "Alice"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.Email, "AliceSmith@email.com"),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                new Claim(JwtClaimTypes.Address,
                    @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                    IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json)
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Console.WriteLine("alice created");
        }
        else
        {
            Console.WriteLine("alice already exists");
        }

        var bob = userMgr.FindByNameAsync("bob").Result;
        if (bob == null)
        {
            bob = new ApplicationUser
            {
                UserName = "bob"
            };
            var result = userMgr.CreateAsync(bob, "Pass123$").Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            result = userMgr.AddClaimsAsync(bob, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, "Bob Smith"),
                new Claim(JwtClaimTypes.GivenName, "Bob"),
                new Claim(JwtClaimTypes.FamilyName, "Smith"),
                new Claim(JwtClaimTypes.Email, "BobSmith@email.com"),
                new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                new Claim(JwtClaimTypes.Address,
                    @"{ 'street_address': 'One Hacker Way', 'locality': 'Heidelberg', 'postal_code': 69118, 'country': 'Germany' }",
                    IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                new Claim("location", "somewhere")
            }).Result;
            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }

            Console.WriteLine("bob created");
        }
        else
        {
            Console.WriteLine("bob already exists");
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}