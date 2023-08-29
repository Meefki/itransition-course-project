using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityServer.Data;

internal class IdentityUserConfiguration
    : IEntityTypeConfiguration<IdentityUser>
{
    public void Configure(EntityTypeBuilder<IdentityUser> builder)
    {
        builder.HasKey(x => x.Id);
    }
}