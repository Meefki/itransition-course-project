using Reviewing.Domain.AggregateModels.CommentAggregate.DomainEvents;
using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.AggregateModels.CommentAggregate;

public sealed class Comment
    : Entity<Guid>, IAggregateRoot
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Comment() : base(null!) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Comment(
        ReviewId reviewId,
        UserId userId,
        string text)
        : base(CommentId.Create<CommentId>(Guid.NewGuid()))
    {
        ReviewId = reviewId;
        UserId = userId;
        Text = text;
        SentDate = DateTime.UtcNow;

        AddCommentAddedDomainEvent(id, reviewId);
    }

    private CommentId id = null!;
    public override EntityIdentifier<Guid> Id
    {
        get => id;
        init => id = (CommentId)value;
    }

    public ReviewId ReviewId { get; init; }
    public UserId UserId { get; init; }
    public string Text { get; init; }
    public DateTime SentDate { get; init; }

    public void Delete()
    {
        AddCommendDeletedDomainEvent(id, ReviewId);
    }

    #region Domain Events

    private void AddCommentAddedDomainEvent(CommentId commentId, ReviewId reviewId)
    {
        CommentAddedDomainEvent domainEvent = new(commentId, reviewId);
        AddDomainEvent(domainEvent);
    }

    private void AddCommendDeletedDomainEvent(CommentId commentId, ReviewId reviewId)
    {
        CommentDeletedDomainEvent domainEvent = new(commentId, reviewId);
        AddDomainEvent(domainEvent);
    }

    #endregion
}