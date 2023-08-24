using Reviewing.API.Application.Queries.Options;

namespace Reviewing.API.Application.Queries;

public interface IReviewQueries
{
    Task<dynamic> GetTags(string startWith = "");
    Task<dynamic> GetReviewsDescription(
        PaginationOptions paginationOptions,
        ReviewSortOptions sortOptions,
        ReviewFilterOptions filterOptions);

    Task<dynamic> GetReview(string reviewId);
}
