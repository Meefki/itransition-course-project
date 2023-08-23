using MediatR;
using Reviewing.Application.Commands.CommentCommands;

namespace Reviewing.API.Application.Commands.CommentCommands;

public record RemoveCommentCommand(string CommentId) :
    RemoveCommentAbstractCommand(CommentId),
    IRequest<CommandResponse>;
