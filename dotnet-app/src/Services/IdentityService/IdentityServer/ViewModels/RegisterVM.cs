namespace IdentityServer.ViewModels;

public class RegisterVM
{
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    //public string Name { get; set; } = null!;
}