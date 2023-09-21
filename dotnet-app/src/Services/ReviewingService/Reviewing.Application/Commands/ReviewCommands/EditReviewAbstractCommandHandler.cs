using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Application.Services;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Commands.ReviewCommands;

public abstract class EditReviewAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest>
    where TRequest : EditReviewAbstractCommand
{
    private readonly IReviewRepository reviewRepository;
    private readonly IImageService imageService;

    protected EditReviewAbstractCommandHandler(
        IReviewRepository reviewRepository,
        IImageService imageService,
        ILogger logger)
        : base(logger)
    {
        this.reviewRepository = reviewRepository;
        this.imageService = imageService;
    }

    protected override async Task Action(TRequest request, CancellationToken cancellationToken)
    {
        ReviewId reviewId = ReviewId.Create<ReviewId>(Guid.Parse(request.ReviewId));
        Review review = await reviewRepository.GetById(reviewId);

        bool isChanged = await UpdateReview(review, request);

        if (isChanged)
        {
            await reviewRepository.Update(review);
            await reviewRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }

    private async Task<bool> UpdateReview(Review review, TRequest request)
    {
        bool isChanged = false;

        SubjectGroups subjectGroup = new(1, request.SubjectName);
        Subject subject = Subject.Create(SubjectId.Create<SubjectId>(Guid.Parse(request.SubjectId)), request.SubjectName, subjectGroup, request.SubjectGrade);
        if (review.Subject != subject)
        {
            review.ChangeSubject(subject);
            isChanged = true;
        }

        if (review.Name != request.Name)
        {
            review.ChangeName(request.Name);
            isChanged = true;
        }

        if (review.Content != request.Content)
        {
            review.ChangeContent(request.Content);
            isChanged = true;
        }

        if (review.ShortDesc != request.ShortDesc)
        {
            review.ChangeShortDesc(request.ShortDesc);
            isChanged = true;
        }

        if (!string.IsNullOrEmpty(request.ImageContentType) && request.ImageInputStream is not null)
        {
            string? imageUrl = await imageService.UploadImageAsync(review.Id.ToString(), request.ImageContentType, request.ImageInputStream);
            review.ChangeImage(imageUrl);
            isChanged = true;
        }
        else
        {
            review.ChangeImage(null);
            isChanged = true;
        }

        HashSet<Tag> tags = request.Tags.Select(t => new Tag(t)).ToHashSet();
        if (review.Tags.ToHashSet().SetEquals(tags))
        {
            review.ChangeTags(tags);
            isChanged = true;
        }

        return isChanged;
    }
}