using Comments.Domain.DomainEvents;
using Comments.Domain.SeedWork;

namespace Comments.Domain;

public class Comment 
    : Entity<Guid>, IAggregateRoot
{
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

        AddCommentAddedDomainEvent(id);
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
        AddCommendDeletedDomainEvent(id);
    }

    #region Domain Events

    private void AddCommentAddedDomainEvent(CommentId commentId)
    {
        CommentAddedDomainEvent domainEvent = new(commentId);
        AddDomainEvent(domainEvent);
    }

    private void AddCommendDeletedDomainEvent(CommentId commentId)
    {
        CommentDeletedDomainEvent domainEvent = new(id);
        AddDomainEvent(domainEvent);
    }

    #endregion
}