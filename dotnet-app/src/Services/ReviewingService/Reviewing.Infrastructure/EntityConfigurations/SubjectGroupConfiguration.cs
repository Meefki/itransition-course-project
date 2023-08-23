using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviewing.Domain.Enumerations;

namespace Reviewing.Infrastructure.EntityConfigurations;

public class SubjectGroupConfiguration
    : IEntityTypeConfiguration<SubjectGroups>
{
    public void Configure(EntityTypeBuilder<SubjectGroups> builder)
    {
        builder.ToTable("SubjectGroups", ReviewingDbContext.DEFAULT_SCHEMA);

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasDefaultValue(1)
            .ValueGeneratedNever()
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}