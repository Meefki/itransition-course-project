namespace Reviewing.API.Application.Queries;

public interface ICommentQueries
{
    Task<dynamic> GetReviewComments(string reviewId, int pageSize, int pageNumber);
}
