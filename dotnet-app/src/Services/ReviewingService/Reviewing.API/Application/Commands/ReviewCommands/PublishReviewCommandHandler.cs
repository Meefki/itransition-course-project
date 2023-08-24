using MediatR;
using Reviewing.Application.Commands.ReviewCommands;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public class PublishReviewCommandHandler :
    PublishReviewAbstractCommandHandler<PublishReviewCommand>,
    IRequestHandler<PublishReviewCommand, CommandResponse<string>>
{
    public PublishReviewCommandHandler(
        IReviewRepository reviewRepository, 
        ILogger<PublishReviewCommandHandler> logger) 
        : base(reviewRepository, logger)
    { }
}