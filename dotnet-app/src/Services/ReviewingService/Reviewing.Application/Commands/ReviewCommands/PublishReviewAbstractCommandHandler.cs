using Microsoft.Extensions.Logging;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;
using Reviewing.Domain.SeedWork;

namespace Reviewing.Application.Commands.ReviewCommands;

public abstract class PublishReviewAbstractCommandHandler<TRequest>
    : CommandHandler<TRequest, string>
    where TRequest : PublishReviewAbstractCommand
{
    private readonly IReviewRepository reviewRepository;

    protected PublishReviewAbstractCommandHandler(
        IReviewRepository reviewRepository,
        ILogger logger)
        : base(logger)
    {
        this.reviewRepository = reviewRepository;
    }

    protected override async Task<string> Action(TRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<SubjectGroups> subjectGroups = Enumeration.GetAll<SubjectGroups>();
        SubjectGroups subjectGroup = 
            subjectGroups
                .FirstOrDefault(x => x.Name == request.SubjectName) ?? 
                new(subjectGroups.Max(x => x.Id) + 1, request.SubjectName);
        Subject subject = Subject.Create(request.SubjectName, subjectGroup, request.SubjectGrade);
        UserId userId = UserId.Create<UserId>(Guid.Parse(request.AuthorUserId));
        Review review = 
            new(request.Name,
                userId,
                subject, 
                request.Content, 
                request.ShortDesc,
                request.ImageUrl, 
                new HashSet<Tag>(request.Tags.Select(t => new Tag(t))));

        await reviewRepository.Add(review);

        await reviewRepository.UnitOfWork
            .SaveEntitiesAsync(cancellationToken);

        return review.Id.ToString();
    }
}