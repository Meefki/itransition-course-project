namespace Reviewing.API.Application.Queries.ViewModels;

public class ReviewVM
{
    public ReviewVM()
    {
        tags = new();
    }

    public string id { get; set; } = null!;
    public string name { get; set; } = null!;
    public string authorUserId { get; set; } = null!;
    public string content { get; set; } = null!;
    public string status { get; set; } = null!;
    public string? imageUrl { get; set; }
    public string subjectName { get; set; } = null!;
    public int subjectGrade { get; set; }
    public string subjectGroupName { get; set; } = null!;
    public int likesCount { get; set; }
    public List<string> tags { get; set; }

}