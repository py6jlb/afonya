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


