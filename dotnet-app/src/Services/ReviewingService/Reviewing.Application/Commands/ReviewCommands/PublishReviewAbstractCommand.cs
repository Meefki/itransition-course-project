namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record PublishReviewAbstractCommand(
    string Name,
    string AuthorUserId,
    string Content,
    string ShortDesc,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int SubjectGrade,
    IEnumerable<string> Tags);