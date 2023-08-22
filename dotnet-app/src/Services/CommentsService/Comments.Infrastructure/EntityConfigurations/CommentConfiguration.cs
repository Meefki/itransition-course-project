using Comments.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Comments.Infrastructure.EntityConfigurations;

public class CommentConfiguration
    : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments", CommentDbContext.DEFAULT_SCHEMA);

        builder.Ignore(c => c.DomainEvents);

        builder.Property(c => c.Id)
            .HasConversion(
                id => id.Value.ToString(),
                value => CommentId.Create<CommentId>(Guid.Parse(value)));
        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .HasConversion(
                id => id.Value.ToString(),
                value => UserId.Create<UserId>(Guid.Parse(value)));

        builder.Property(c => c.ReviewId)
            .HasConversion(
                id => id.Value.ToString(),
                value => ReviewId.Create<ReviewId>(Guid.Parse(value)));
    }
}