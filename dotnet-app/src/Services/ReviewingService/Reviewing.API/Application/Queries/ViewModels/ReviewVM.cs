namespace Reviewing.API.Application.Queries.ViewModels;

public class ReviewVM
{
    public ReviewVM()
    {
        tags = new();
    }

    public Guid id { get; set; }
    public string name { get; set; } = null!;
    public Guid authorUserId { get; set; }
    public string shortDesc { get; set; } = null!;
    public string status { get; set; } = null!;
    public string? imageUrl { get; set; }
    public string subjectName { get; set; } = null!;
    public int subjectGrade { get; set; }
    public string subjectGroupName { get; set; } = null!;
    public int? likesCount { get; set; }
    public DateTime publishedDate { get; set; }
    public List<string> tags { get; set; }
    public string content { get; set; } = null!;
    public float estimate { get; set; }
    public bool isLiked { get; set; } = false;
    public int userEstimate { get; set; }

}