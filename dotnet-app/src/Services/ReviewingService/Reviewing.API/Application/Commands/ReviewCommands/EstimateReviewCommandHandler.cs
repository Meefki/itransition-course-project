using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public class EstimateReviewCommandHandler :
    EstimateReviewAbstractCommandHandler<EstimateReviewCommand>,
    IRequestHandler<EstimateReviewCommand, CommandResponse>
{
    public EstimateReviewCommandHandler(
        IReviewRepository reviewRepository, 
        ILogger<EstimateReviewCommandHandler> logger) 
        : base(reviewRepository, logger)
    { }
}