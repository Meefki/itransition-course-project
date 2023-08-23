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
                id => id.ToString(),
                value => ReviewId.Create<ReviewId>(Guid.Parse(value)));
        builder.HasKey(x => x.Id);

        builder.Ignore(x => x.Status);
        builder.Ignore(x => x.Subject);
        builder.Ignore(x => x.Comments);
        builder.Ignore(x => x.Likes);
        builder.Ignore(x => x.Tags);

        //builder.Property(x => x.Status)
        //    .HasConversion(
        //        status => status.Name,
        //        value => Enumeration.FromDisplayName<ReviewStatuses>(value));

        //builder.OwnsOne(x => x.Subject, subjectBuidler =>
        //{
        //    subjectBuidler.Property(x => x.Group)
        //        .HasConversion(
        //            group => group.Name,
        //            value => Enumeration.FromDisplayName<SubjectGroups>(value));
        //});

        //builder.OwnsMany(x => x.Comments, commentBuilder =>
        //{
        //    commentBuilder.ToTable("ReviewComments", ReviewingDbContext.DEFAULT_SCHEMA);
        //    const string foreignKey = "ReviewId";
        //    const string primaryKey = "CommentId";
        //    commentBuilder.Property<Guid>(foreignKey);
        //    commentBuilder.WithOwner().HasForeignKey(foreignKey);
        //    commentBuilder.Property(x => x.Value).HasColumnName(primaryKey);
        //    commentBuilder.HasKey(primaryKey, foreignKey);
        //});

        //builder.OwnsMany(x => x.Likes, likeBuilder =>
        //{
        //    likeBuilder.ToTable("ReviewLikes", ReviewingDbContext.DEFAULT_SCHEMA);
        //    const string foreignKey = "ReviewId";
        //    const string primaryKey = "UserId";
        //    likeBuilder.Property<Guid>(foreignKey);
        //    likeBuilder.WithOwner().HasForeignKey(foreignKey);
        //    likeBuilder.Property(x => x.Value).HasColumnName(primaryKey);
        //    likeBuilder.HasKey(primaryKey, foreignKey);
        //});

        //builder.HasMany(x => x.Tags)
        //    .WithMany();

        //builder.OwnsMany(x => x.Tags, tagBuilder =>
        //{
        //    tagBuilder.ToTable("ReviewTags");
        //    const string foreignKey = "ReviewId";
        //    const string primaryKey = "Name";
        //    tagBuilder.Property<Guid>(foreignKey);
        //    tagBuilder.WithOwner().HasForeignKey(foreignKey);        
        //    tagBuilder.HasKey(primaryKey, foreignKey);
        //});
    }
}