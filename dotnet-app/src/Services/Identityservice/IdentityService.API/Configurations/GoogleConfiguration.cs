namespace IdentityService.Configurations;

public class GoogleConfiguration
{
    public static string SectionName => nameof(GoogleConfiguration);

    public string ClientId { get; set; } = null!;
    public string ClientSecret { get; set; } = null!;
}