using Microsoft.EntityFrameworkCore;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
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
}