using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Application.Services;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Commands.ReviewCommands;

public abstract class PublishReviewAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest, string>
    where TRequest : PublishReviewAbstractCommand
{
    private readonly IReviewRepository reviewRepository;
    private readonly IImageService imageService;

    protected PublishReviewAbstractCommandHandler(
        IReviewRepository reviewRepository,
        IImageService imageService,
        ILogger logger)
        : base(logger)
    {
        this.reviewRepository = reviewRepository;
        this.imageService = imageService;
    }

    protected override async Task<string> Action(TRequest request, CancellationToken cancellationToken)
    {
        SubjectId subjectId = SubjectId.Create<SubjectId>(Guid.Parse(request.SubjectId));
        Subject subject = (await reviewRepository.GetSubject(subjectId))!;
        UserId userId = UserId.Create<UserId>(Guid.Parse(request.AuthorUserId));
        Review review = 
            new(request.Name,
                userId,
                subject, 
                request.Content, 
                request.ShortDesc,
                null,
                new HashSet<Tag>(request.Tags.Select(t => new Tag(t))));
        string? imageUrl = await imageService.UploadImageAsync(review.Id.ToString(), request.ImageContentType, request.ImageInputStream);
        review.ChangeImage(imageUrl);

        await reviewRepository.Add(review);

        await reviewRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);

        return review.Id.ToString();
    }
}