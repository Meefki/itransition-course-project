using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviewing.Domain.AggregateModels.ReviewAggregate;

namespace Reviewing.Infrastructure.EntityConfigurations;

public class TagConfiguration
    : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tags", ReviewingDbContext.DEFAULT_SCHEMA);
        builder.HasKey(x => x.Name);
    }
}