using IdentityServer4.Models;

namespace Afonya.Identity.Web.Config;

public class Config
{
    public IEnumerable<ApiScopeDto> ApiScopes { get; set; }
    public IEnumerable<ClientDto> Clients { get; set; }

    public IEnumerable<ApiScope> GetApiScopes()
    {
        return ApiScopes.Select(x => new ApiScope(x.Name, x.ToString()));
    }

    public IEnumerable<Client> GetClients()
    {
        return Clients.Select(x => new Client()
        {
            ClientId = x.ClientId,
            AllowedGrantTypes  = CalcGrantType(x.AllowedGrantType),
            ClientSecrets =
            {
                new Secret(x.ClientSecret.Sha256())
            },
            AllowedScopes = x.AllowedScopes.ToArray()
        });
    } 
    
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            // You may add other identity resources like address,phone... etc
            //new IdentityResources.Address()
        };
    }
    
    public IEnumerable<ApiResource> GetApis()
    {
        return new ApiResource[]
        {
            new ApiResource("identity.api", "Identity API"),
            new ApiResource("test.api","Test API")
        };
    }
    
    private ICollection<string> CalcGrantType(string type)
    {
        return type switch
        {
            "implicit" => GrantTypes.Implicit,
            "client_credentials" => GrantTypes.ClientCredentials,
            _ => Array.Empty<string>()
        };
    }
}


