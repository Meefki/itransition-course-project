using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviewing.Domain.AggregateModels.CommentAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Infrastructure.EntityConfigurations;

public class CommentConfiguration
    : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments", ReviewingDbContext.DEFAULT_SCHEMA);

        builder.Ignore(c => c.DomainEvents);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value,
                value => CommentId.Create<CommentId>(value));
        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .HasConversion(
                id => id.Value,
                value => UserId.Create<UserId>(value));

        builder.Property(c => c.ReviewId)
            .HasConversion(
                id => id.Value,
                value => ReviewId.Create<ReviewId>(value));
    }
}