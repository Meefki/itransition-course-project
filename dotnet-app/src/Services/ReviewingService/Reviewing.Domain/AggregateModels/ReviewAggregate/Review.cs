using Reviewing.Domain.AggregateModels.ReviewAggregate.DomainExceptions;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate;

public sealed class Review
    : Entity<Guid>, IAggregateRoot
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private Review() : base(null!) { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public Review(
        string name,
        Subject subject,
        string content,
        string image,
        IEnumerable<Tag> tags)
        : base(ReviewId.Create<ReviewId>(Guid.NewGuid()))
    {
        Name = name;
        Subject = subject;
        Content = content;
        Image = image;
        Status = ReviewStatuses.Draft;

        this.tags = new HashSet<Tag>(tags);
        comments = new HashSet<CommentId>();
        likes = new HashSet<UserId>();
    }

    private ReviewId id = null!;
    public override EntityIdentifier<Guid> Id
    {
        get => id;
        init => id = (ReviewId)value;
    }

    public string Name { get; private set; }
    public void ChangeName(string name)
    {
        if (!IsDraft())
            CannotChangeReviewInfoDomainException.Throw();
        Name = name;
    }

    public Subject Subject { get; private set; }
    public void ChangeSubject(Subject subject)
    {
        if (!IsDraft())
            CannotChangeReviewInfoDomainException.Throw();
        Subject = subject;
    }

    public string Content { get; private set; }
    public void ChangeContent(string content)
    {
        if (!IsDraft())
            CannotChangeReviewInfoDomainException.Throw();
        Content = content;
    }

    public string Image { get; private set; }
    public void ChangeImage(string image)
    {
        if (!IsDraft())
            CannotChangeReviewInfoDomainException.Throw();
        Image = image;
    }

    public ReviewStatuses Status { get; private set; }
    public void ChangeStatus(ReviewStatuses newStatus)
    {
        if (newStatus.Id <= Status.Id)
            ConnotChangeReviewStatusDomainException.Throw(Status, newStatus);
        Status = newStatus;
    }

    private readonly ISet<Tag> tags;
    public IReadOnlyCollection<Tag> Tags => tags.ToList().AsReadOnly();
    public void ChangeTags(IEnumerable<Tag> tags)
    {
        if (!IsDraft())
            CannotChangeReviewInfoDomainException.Throw();
        this.tags.Clear();
        foreach (var tag in tags)
            this.tags.Add(tag);
    }

    private readonly ISet<CommentId> comments;
    public IReadOnlyCollection<CommentId> Comments => comments.ToList().AsReadOnly();
    public void AddComment(CommentId comment)
    {
        comments.Add(comment);
    }
    public void RemoveComment(CommentId commentId)
    {
        comments.Remove(commentId);
    }

    private readonly ISet<UserId> likes;
    public IReadOnlyCollection<UserId> Likes => likes.ToList().AsReadOnly();
    public void ChangeLike(UserId userId)
    {
        if (likes.Contains(userId))
            likes.Remove(userId);
        else
            likes.Add(userId);
    }

    private bool IsDraft()
        => Status == ReviewStatuses.Draft;
}