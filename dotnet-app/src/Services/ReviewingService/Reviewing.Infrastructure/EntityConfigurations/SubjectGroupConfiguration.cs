using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;

namespace Reviewing.Infrastructure.EntityConfigurations;

public class SubjectGroupConfiguration 
    : IEntityTypeConfiguration<SubjectGroups>
{
    public void Configure(EntityTypeBuilder<SubjectGroups> builder)
    {
        builder.ToTable("SubjectGroups", ReviewingDbContext.DEFAULT_SCHEMA);

        //builder.HasMany<Review>()
        //    .WithOne()
        //    .HasForeignKey("Subject_GroupId");

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired();
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .HasMaxLength(50)
            .IsRequired();
        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}