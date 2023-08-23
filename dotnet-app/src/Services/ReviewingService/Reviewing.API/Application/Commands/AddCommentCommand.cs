using MediatR;
using Reviewing.Application.Commands;
using Reviewing.Application.SeedWork;

namespace Reviewing.API.Application.Commands;

public record AddCommentCommand(string ReviewId, string UserId, string Text) :
    AddCommentAbstractCommand(ReviewId, UserId, Text),
    IRequest<CommandResponse<string>>;
