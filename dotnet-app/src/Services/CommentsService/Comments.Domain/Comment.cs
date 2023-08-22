namespace Comments.Domain;

public class Comment
{
    public Comment(
        CommentId id,
        ReviewId reviewId,
        UserId userId,
        string text)
    {
        Id = id;
        ReviewId = reviewId;
        UserId = userId;
        Text = text;
        SentDate = DateTime.UtcNow;
    }

    public CommentId Id { get; init; }
    public ReviewId ReviewId { get; init; }
    public UserId UserId { get; init; }
    public string Text { get; init; }
    public DateTime SentDate { get; init; }
}