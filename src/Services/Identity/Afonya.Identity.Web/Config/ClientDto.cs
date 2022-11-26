namespace Afonya.Identity.Web.Config;

public class ClientDto
{
    public string ClientId { get; set; }
    public string AllowedGrantType { get; set; }
    public string ClientSecret { get; set; }
    public IEnumerable<string> AllowedScopes { get; set; }
}