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
                status => status.Id,
                value => Enumeration.FromValue<ReviewStatuses>(value));

        builder.OwnsOne(x => x.Subject, subjectBuidler =>
        {
            subjectBuidler.OwnsOne(x => x.Group, groupBuilder =>
            {
                groupBuilder.ToTable("SubjectGroups", ReviewingDbContext.DEFAULT_SCHEMA);
                const string foreignKey = "ReviewId";
                groupBuilder.WithOwner().HasForeignKey(foreignKey);
                groupBuilder.Property(x => x.Id)
                    .ValueGeneratedNever()
                    .IsRequired();
                groupBuilder.HasKey(x => x.Id);

                groupBuilder.Property(x => x.Name)
                    .HasMaxLength(50)
                    .IsRequired();
                groupBuilder.HasIndex(x => x.Name)
                    .IsUnique();
            });
        });

        builder.OwnsMany(x => x.Comments, commentBuilder =>
        {
            commentBuilder.ToTable("ReviewComments", ReviewingDbContext.DEFAULT_SCHEMA);
            const string foreignKey = "ReviewId";
            const string primaryKey = "CommentId";
            commentBuilder.Property<EntityIdentifier<Guid>>(foreignKey)
                .HasConversion(
                    id => id.Value,
                    value => ReviewId.Create<ReviewId>(value));
            commentBuilder.WithOwner().HasForeignKey(foreignKey);
            commentBuilder.Property(x => x.Value).HasColumnName(primaryKey);
            commentBuilder.HasKey(nameof(CommentId.Value), foreignKey);
        });

        builder.OwnsMany(x => x.Likes, likeBuilder =>
        {
            likeBuilder.ToTable("ReviewLikes", ReviewingDbContext.DEFAULT_SCHEMA);
            const string foreignKey = "ReviewId";
            const string primaryKey = "UserId";
            likeBuilder.Property<EntityIdentifier<Guid>>(foreignKey)
                .HasConversion(
                    id => id.Value,
                    value => ReviewId.Create<ReviewId>(value));
            likeBuilder.WithOwner().HasForeignKey(foreignKey);
            likeBuilder.Property(x => x.Value).HasColumnName(primaryKey);
            likeBuilder.HasKey(nameof(UserId.Value), foreignKey);
        });

        builder.HasMany(x => x.Tags)
            .WithMany();
    }
}