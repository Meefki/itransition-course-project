using Reviewing.API.Application.Queries.Options;

namespace Reviewing.API.Application.Queries;

public interface IReviewQueries
{
    Task<dynamic> GetTags(string startWith = "");
    Task<dynamic> GetMostPopularTags(int pageSize = 10);
    Task<dynamic> GetSubjects(string startWith);
    Task<dynamic> GetSubjectGroups();
    Task<dynamic> GetReviewsShortDescription(
        PaginationOptions paginationOptions,
        List<ReviewSortOptions>? sortOptions = null,
        ReviewFilterOptions? filterOptions = null,
        string? userId = null);

    Task<dynamic> GetReview(string reviewId, string? userId = null);

    Task<dynamic> GetReviewsCount(ReviewFilterOptions? filterOptions = null);
}
