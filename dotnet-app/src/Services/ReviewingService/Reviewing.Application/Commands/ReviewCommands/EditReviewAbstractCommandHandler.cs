using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Application.Commands.ReviewCommands;

public abstract class EditReviewAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest>
    where TRequest : EditReviewAbstractCommand
{
    private readonly IReviewRepository reviewRepository;

    protected EditReviewAbstractCommandHandler(
        IReviewRepository reviewRepository,
        ILogger logger)
        : base(logger)
    {
        this.reviewRepository = reviewRepository;
    }

    protected override async Task Action(TRequest request, CancellationToken cancellationToken)
    {
        ReviewId reviewId = ReviewId.Create<ReviewId>(Guid.Parse(request.ReviewId));
        Review review = await reviewRepository.GetById(reviewId);

        bool isChanged = EditReviewAbstractCommandHandler<TRequest>.UpdateReview(review, request);

        if (isChanged)
        {
            await reviewRepository.Update(review);
            await reviewRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);
        }
    }

    private static bool UpdateReview(Review review, TRequest request)
    {
        bool isChanged = false;

        IEnumerable<SubjectGroups> subjectGroups = Enumeration.GetAll<SubjectGroups>();
        SubjectGroups subjectGroup =
            subjectGroups
                .FirstOrDefault(x => x.Name == request.SubjectName) ??
                new(subjectGroups.Max(x => x.Id) + 1, request.SubjectName);
        Subject subject = Subject.Create(request.SubjectName, subjectGroup, request.SubjectGrade);
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

        if (review.ImageUrl != request.ImageUrl)
        {
            review.ChangeImage(request.ImageUrl);
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