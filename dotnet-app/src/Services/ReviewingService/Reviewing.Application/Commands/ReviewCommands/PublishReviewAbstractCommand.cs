namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record PublishReviewAbstractCommand(
    string Name,
    string AuthorUserId,
    string Content,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int SubjectGrade,
    IEnumerable<string> Tags);