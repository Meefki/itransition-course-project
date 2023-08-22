namespace Users.Domain;

public class User
{
    public User()
    {
        Id = new(Guid.NewGuid());

        writtenReviews = new HashSet<ReviewId>();
        likedReviews = new HashSet<ReviewId>();
        comments = new HashSet<CommentId>();
    }

    public UserId Id { get; }

    private readonly ISet<ReviewId> writtenReviews;
    public IReadOnlyCollection<ReviewId> WrittenReviews => writtenReviews.ToList().AsReadOnly();

    private readonly ISet<ReviewId> likedReviews;
    public IReadOnlyCollection<ReviewId> LikedReviews => likedReviews.ToList().AsReadOnly();

    private readonly ISet<CommentId> comments;
    public IReadOnlyCollection<CommentId> Comments => comments.ToList().AsReadOnly();

    public void AddReview(ReviewId reviewId)
    {
        writtenReviews.Add(reviewId);
    }

    public void RemoveReview(ReviewId reviewId)
    {
        writtenReviews.Remove(reviewId);
    }

    public void AddComment(CommentId commentId)
    {
        comments.Add(commentId);
    }

    public void RemoveComment(CommentId commentId)
    {
        comments.Remove(commentId);
    }
}