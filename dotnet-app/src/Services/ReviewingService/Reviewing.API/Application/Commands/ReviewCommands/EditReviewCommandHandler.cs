using MediatR;
using Reviewing.Application.Commands.ReviewCommands;
using Reviewing.Application.Services;

namespace Reviewing.API.Application.Commands.ReviewCommands;

public class EditReviewCommandHandler :
    EditReviewAbstractCommandHandler<EditReviewCommand>,
    IRequestHandler<EditReviewCommand, CommandResponse>
{
    public EditReviewCommandHandler(
        IReviewRepository reviewRepository, 
        IImageService imageService,
        ILogger<EditReviewCommandHandler> logger) 
        : base(reviewRepository, imageService, logger)
    { }
}