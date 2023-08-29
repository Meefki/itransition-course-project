using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;

public class AuthorizationDbContext
    : IdentityDbContext<IdentityUser>
{
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityUser>(entity => entity.ToTable(name: "Users"));
        builder.Entity<IdentityRole>(entity => entity.ToTable(name: "Roles"));
        builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"));
        builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable(name: "UserClaims"));
        builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable(name: "UserLogins"));
        builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable(name: "UserTokens"));
        builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable(name: "RoleClaims"));

        builder.ApplyConfiguration(new IdentityUserConfiguration());
    }
}