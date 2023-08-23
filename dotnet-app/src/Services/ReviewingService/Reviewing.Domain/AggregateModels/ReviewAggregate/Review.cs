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
        string? imageUrl,
        ISet<Tag> tags)
        : base(ReviewId.Create<ReviewId>(Guid.NewGuid()))
    {
        Name = name;
        Subject = subject;
        Content = content;
        ImageUrl = imageUrl;
        Status = ReviewStatuses.Published;

        this.tags = tags;
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
        Name = name;
    }

    public Subject Subject { get; private set; }
    public void ChangeSubject(Subject subject)
    {
        Subject = subject;
    }

    public string Content { get; private set; }
    public void ChangeContent(string content)
    {
        Content = content;
    }

    public string? ImageUrl { get; private set; }
    public void ChangeImage(string imageUrl)
    {
        //if (!string.IsNullOrWhiteSpace(ImageUrl) && 
        //    string.IsNullOrWhiteSpace(imageUrl))
        //    AddDomainEvent...

        ImageUrl = imageUrl;
    }

    public ReviewStatuses Status { get; private set; }
    public void ChangeStatus(ReviewStatuses newStatus)
    {
        Status = newStatus;
    }

    private readonly ISet<Tag> tags;
    public IReadOnlyCollection<Tag> Tags => tags.ToList().AsReadOnly();
    public void ChangeTags(IEnumerable<Tag> tags)
    {
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
}