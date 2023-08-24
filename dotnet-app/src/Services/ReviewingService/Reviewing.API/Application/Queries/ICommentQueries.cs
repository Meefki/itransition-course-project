using Reviewing.API.Application.Queries.Options;
using Reviewing.API.Application.Queries.ViewModels;

namespace Reviewing.API.Application.Queries;

public interface ICommentQueries
{
    Task<dynamic> GetReviewComments(string reviewId, PaginationOptions? pagination = null);
}
