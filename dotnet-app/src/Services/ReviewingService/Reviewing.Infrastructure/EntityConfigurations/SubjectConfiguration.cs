using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Infrastructure.EntityConfigurations;

public class SubjectConfiguration
    : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        builder.ToTable("Subjects", ReviewingDbContext.DEFAULT_SCHEMA);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => SubjectId.Create<SubjectId>(value));
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Group)
            .WithMany()
            .HasForeignKey("GroupId");
    }
}