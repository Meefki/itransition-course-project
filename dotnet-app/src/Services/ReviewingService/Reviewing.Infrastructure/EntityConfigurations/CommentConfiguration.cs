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
                id => id.ToString(),
                value => CommentId.Create<CommentId>(Guid.Parse(value)));
        builder.HasKey(c => c.Id);

        builder.Ignore(x => x.UserId);
        builder.Ignore(x => x.ReviewId);

        //builder.Property(c => c.UserId)
        //    .HasConversion(
        //        id => id.ToString(), 
        //        value => UserId.Create<UserId>(Guid.Parse(value)));

        //builder.Property(c => c.ReviewId)
        //    .HasConversion(
        //        id => id.ToString(), 
        //        value => ReviewId.Create<ReviewId>(Guid.Parse(value)));
    }
}