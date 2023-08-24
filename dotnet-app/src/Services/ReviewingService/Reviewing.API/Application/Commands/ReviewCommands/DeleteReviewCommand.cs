using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public record DeleteReviewCommand(string ReviewId)
    : DeleteReviewAbstractCommand(ReviewId), IRequest<CommandResponse>;