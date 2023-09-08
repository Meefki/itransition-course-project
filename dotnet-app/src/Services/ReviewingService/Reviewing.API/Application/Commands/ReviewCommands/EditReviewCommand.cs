using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public record EditReviewCommand(
    string ReviewId,
    string Name,
    string Content,
    string ShortDesc,
    string? ImageUrl,
    string SubjectName,
    string SubjectGroupName,
    int Grade,
    IEnumerable<string> Tags) : 
    EditReviewAbstractCommand(
        ReviewId,
        Name,
        Content,
        ShortDesc,
        ImageUrl,
        SubjectName,
        SubjectGroupName,
        Grade,
        Tags), 
    IRequest<CommandResponse>;