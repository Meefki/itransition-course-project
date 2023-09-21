using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public record EstimateReviewCommand(
    string ReviewId,
    string UserId,
    int Grade) :
    EstimateReviewAbstractCommand(
        ReviewId,
        UserId,
        Grade),
    IRequest<CommandResponse>;