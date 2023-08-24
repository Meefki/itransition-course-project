using MediatR;

namespace Reviewing.Application.Commands.ReviewCommands;

public record DeleteReviewCommand(string ReviewId) 
    : DeleteReviewAbstractCommand(ReviewId), IRequest<CommandResponse>;