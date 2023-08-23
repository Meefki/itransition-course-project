using MediatR;
using Reviewing.Application.Commands;
using Reviewing.Application.SeedWork;

namespace Reviewing.API.Application.Commands;

public record RemoveCommentCommand(string CommentId) :
    RemoveCommentAbstractCommand(CommentId),
    IRequest<CommandResponse>;
