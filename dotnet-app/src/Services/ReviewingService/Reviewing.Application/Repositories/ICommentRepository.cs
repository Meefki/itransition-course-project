using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.CommentAggregate;
using Reviewing.Domain.Identifiers;

namespace Reviewing.Application.Repositories;

public interface ICommentRepository
    : IRepository<Comment>
{
    Task<Comment> GetById(CommentId id);
    Task Add(Comment comment);
    Task Delete(Comment comment);
}