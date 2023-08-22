using Comments.Domain;
using Comments.Domain.SeedWork;

namespace Comments.Application.Repositories;

public interface ICommentRepository 
    : IRepository<Comment>
{
    Task<Comment> Get(CommentId id);
    Task Create(Comment comment);
    Task Delete(Comment comment);
}