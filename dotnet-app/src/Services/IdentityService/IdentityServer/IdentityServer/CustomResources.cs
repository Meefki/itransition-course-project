using IdentityModel;
using IdentityServer4.Models;

namespace IdentityServer.IdentityServer;

public static class CustomResources
{
    public static IdentityResource Role
    {
        get
        {
            var role = new IdentityResource()
            {
                Required = false,
                Name = "role",
                DisplayName = "Role",
                Description = "User role",
                Emphasize = true,
                Enabled = true,
                ShowInDiscoveryDocument = true,
                UserClaims = new string[] { JwtClaimTypes.Role }
            };

            return role;
        }
    }
}