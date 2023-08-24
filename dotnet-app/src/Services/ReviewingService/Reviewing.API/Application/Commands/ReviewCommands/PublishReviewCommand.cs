using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public record PublishReviewCommand(
    string Name,
    string AuthorUserId,
    string Content,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int SubjectGrade,
    IEnumerable<string> Tags) : 
    PublishReviewAbstractCommand(
        Name,
        AuthorUserId,
        Content,
        ImageUrl,
        SubjectName,
        SubjectGroupName,
        SubjectGrade,
        Tags),
    IRequest<CommandResponse<string>>;