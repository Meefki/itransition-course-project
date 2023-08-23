namespace Reviewing.Application.Commands.ReviewCommands;

public abstract record PublishReviewAbstractCommand(
    string Name,
    string Content,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int Grade,
    IEnumerable<string> Tags);