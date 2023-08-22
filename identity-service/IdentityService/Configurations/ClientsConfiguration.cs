namespace IdentityService.Configurations;

internal class ClientsConfiguration
{
    public static string SectionName => "Clients";

    public ClientsConfiguration()
    {
        RedirectUris = new List<string>();
        AllowedCorsOrigins = new List<string>();
        PostLogoutRedirectUris = new List<string>();
    }

    public ICollection<string> RedirectUris { get; init; }
    public ICollection<string> AllowedCorsOrigins { get; init; }
    public ICollection<string> PostLogoutRedirectUris { get; init; }
}