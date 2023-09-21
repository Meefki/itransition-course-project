using MediatR;
using Reviewing.Application.Commands.ReviewCommands;
using Reviewing.Application.Services;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public class PublishReviewCommandHandler :
    PublishReviewAbstractCommandHandler<PublishReviewCommand>,
    IRequestHandler<PublishReviewCommand, CommandResponse<string>>
{
    public PublishReviewCommandHandler(
        IReviewRepository reviewRepository,
        IImageService imageService,
        ILogger<PublishReviewCommandHandler> logger) 
        : base(reviewRepository, imageService, logger)
    { }
}