namespace Reviewing.API.Application.Queries.ViewModels;

public class CommentVM
{
    public string CommentId { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTime SentDate { get; set; }
}