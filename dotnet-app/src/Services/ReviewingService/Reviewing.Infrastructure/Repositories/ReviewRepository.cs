using Microsoft.EntityFrameworkCore;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Infrastructure.Repositories;

public class ReviewRepository
    : IReviewRepository
{
    private readonly ReviewingDbContext context;

    public IUnitOfWork UnitOfWork => context;

    public ReviewRepository(
        ReviewingDbContext context)
    {
        this.context = context;
    }

    public async Task Add(Review review)
    {
        var tags = review.Tags.Where(x => context.Tags.Contains(x));
        foreach (var tag in tags)
        {
            context.Tags.Attach(tag);
        }
        var subject = context.Subjects.AsNoTracking().FirstOrDefault(x => x.Id == review.Subject.Id);

        await context.Reviews.AddAsync(review);
    }

    public async Task<Review> GetById(ReviewId reviewId)
    {
        return await context.Reviews.FirstAsync(x => x.Id == reviewId);
    }

    public async Task Update(Review review)
    {
        context.Entry(review).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task<SubjectGroups?> GetSubjectGroup(string name)
    {
        var sg = await context.SubjectGroups.FirstOrDefaultAsync(x => x.Name == name);
        return sg;
    }

    public async Task<Subject?> GetSubject(SubjectId subjectId)
    {
        var s = await context.Subjects.FirstOrDefaultAsync(x => x.Id == subjectId);
        return s;
    }
}