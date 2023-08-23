using Microsoft.EntityFrameworkCore;
using Reviewing.Application.Repositories;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.CommentAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Infrastructure.Repositories;

public class CommentRepository
    : ICommentRepository
{
    private readonly ReviewingDbContext context;
    public IUnitOfWork UnitOfWork => context;

    public CommentRepository(
        ReviewingDbContext context)
    {
        this.context = context;
    }

    public async Task Add(Comment comment)
    {
        await context.Comments.AddAsync(comment);
    }

    public async Task Delete(Comment comment)
    {
        context.Comments.Remove(comment);
        await Task.CompletedTask;
    }

    public async Task<Comment> GetById(CommentId id)
    {
        return await context.Comments.FirstAsync(x => x.Id == id);
    }
}