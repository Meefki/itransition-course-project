using MediatR;
using Reviewing.Application.Commands.CommentCommands;

namespace Reviewing.API.Application.Commands;

public record AddCommentCommand(string ReviewId, string UserId, string Text) :
    AddCommentAbstractCommand(ReviewId, UserId, Text),
    IRequest<CommandResponse<string>>;
