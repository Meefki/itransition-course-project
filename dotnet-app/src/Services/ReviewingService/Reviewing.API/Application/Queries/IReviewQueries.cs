using Reviewing.API.Application.Queries.Options;

namespace Reviewing.API.Application.Queries;

public interface IReviewQueries
{
    Task<dynamic> GetTags(string startWith = "");
    Task<dynamic> GetReviewsShortDescription(
        PaginationOptions paginationOptions,
        ReviewSortOptions? sortOptions = null,
        ReviewFilterOptions? filterOptions = null);

    Task<dynamic> GetReview(string reviewId);

    Task<dynamic> GetReviewsCount(ReviewFilterOptions? filterOptions = null);
}
