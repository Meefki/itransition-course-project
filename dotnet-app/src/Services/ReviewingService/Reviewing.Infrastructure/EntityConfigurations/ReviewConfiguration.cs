using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Infrastructure.EntityConfigurations;

public class ReviewConfiguration
    : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Reviews", ReviewingDbContext.DEFAULT_SCHEMA);

        builder.Ignore(x => x.DomainEvents);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => ReviewId.Create<ReviewId>(value));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AuthorUserId)
            .HasConversion(
                userId => userId.Value,
                value => UserId.Create<UserId>(value));

        builder.Property(x => x.Status)
            .HasConversion(
                status => status.Name,
                value => Enumeration.FromDisplayName<ReviewStatuses>(value));

        builder.OwnsMany(x => x.Comments, commentBuilder =>
        {
            commentBuilder.ToTable("ReviewComments", ReviewingDbContext.DEFAULT_SCHEMA);
            const string foreignKey = "ReviewId";
            const string primaryKey = "CommentId";
            commentBuilder.WithOwner().HasForeignKey(foreignKey);
            commentBuilder.Property(x => x.Value).HasColumnName(primaryKey);
            commentBuilder.HasKey(nameof(CommentId.Value), foreignKey);
        });

        builder.OwnsMany(x => x.Likes, likeBuilder =>
        {
            likeBuilder.ToTable("ReviewLikes", ReviewingDbContext.DEFAULT_SCHEMA);
            const string foreignKey = "ReviewId";
            const string primaryKey = "UserId";
            likeBuilder.WithOwner().HasForeignKey(foreignKey);
            likeBuilder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create<UserId>(value))
                .HasColumnName(primaryKey);
            likeBuilder.HasKey("Id", foreignKey);
        });

        builder.OwnsMany(x => x.Estimates, estimateBuilder =>
        {
            estimateBuilder.ToTable("Estimates", ReviewingDbContext.DEFAULT_SCHEMA);
            const string foreignKey = "ReviewId";
            estimateBuilder.WithOwner().HasForeignKey(foreignKey);
            estimateBuilder.Property(x => x.Id)
                .HasConversion(
                    id => id.Value,
                    value => UserId.Create<UserId>(value))
                .HasColumnName("UserId");
            estimateBuilder.HasKey(foreignKey, "Id");
        });

        builder.HasOne(x => x.Subject)
            .WithMany()
            .HasForeignKey("SubjectId");

        builder.HasMany(x => x.Tags)
            .WithMany();
    }
}