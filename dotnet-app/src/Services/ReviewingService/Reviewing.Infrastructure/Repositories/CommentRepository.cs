using Comments.Application.Repositories;
using Comments.Domain;
using Comments.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Comments.Infrastructure.Repositories;

public class CommentRepository
    : ICommentRepository
{
    private readonly CommentDbContext context;
    public IUnitOfWork UnitOfWork => context;

    public CommentRepository(
        CommentDbContext context)
    {
        this.context = context;
    }

    public async Task Create(Comment comment)
    {
        await context.Comments.AddAsync(comment);
    }

    public async Task Delete(Comment comment)
    {
        await Task.Run(() => context.Comments.Remove(comment));
    }

    public async Task<Comment> Get(CommentId id)
    {
        return await context.Comments.FirstAsync(x => x.Id == id);
    }
}