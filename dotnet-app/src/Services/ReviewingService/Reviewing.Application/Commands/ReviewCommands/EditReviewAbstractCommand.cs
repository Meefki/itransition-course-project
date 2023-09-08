namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record EditReviewAbstractCommand(
    string ReviewId,
    string Name,
    string Content,
    string ShortDesc,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int SubjectGrade,
    IEnumerable<string> Tags);