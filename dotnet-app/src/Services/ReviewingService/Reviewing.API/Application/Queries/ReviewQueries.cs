using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Extensions;
using Reviewing.API.Application.Queries.Options;
using Reviewing.API.Application.Queries.ViewModels;

namespace Reviewing.API.Application.Queries;

public class ReviewQueries
    : IReviewQueries
{
    private readonly string connectionString;

    public ReviewQueries(
        string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<dynamic> GetReview(string reviewId)
    {
        var timeout = TimeSpan.FromSeconds(10);
        object param = new { reviewId };
        string reviewSql =
@"with likes_count (likesCount) as (
    select count(rl.UserId)
        from [reviewing].ReviewLikes as rl
        where rl.ReviewId = @reviewId
)
select top 1 
            r.Id             as [id]
        ,r.[Name]         as [name]
        ,r.AuthorUserId   as [authorUserId]
        ,r.Content        as content
        ,r.[Status]       as [status]
        ,r.ImageUrl       as imageUrl
        ,r.Subject_Name   as subjectName
        ,r.Subject_Grade  as subjectGrage
        ,sg.[Name]        as subjectGroupName
        ,lc.likesCount
    from [reviewing].Reviews       as r
    join [reviewing].SubjectGroups as sg on sg.ReviewId = r.Id
    cross join likes_count         as lc
    where r.Id = @reviewId";
        string tagsSql =
@"select rt.TagsName as [name]
    from [reviewing].ReviewTag as rt
    where rt.ReviewId = @reviewId";

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        dynamic review = await connection.QueryAsync<dynamic>(reviewSql, param, commandTimeout: timeout.Seconds);
        dynamic reviewTags = await connection.QueryAsync<dynamic>(tagsSql, param, commandTimeout: timeout.Seconds);

        List<string> tags = new();
        foreach (dynamic tag in reviewTags)
            tags.Add(tag.name);
        review.tags = tags;

        return review;
    }

    public async Task<dynamic> GetReviewsCount(ReviewFilterOptions? filterOptions = null)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var timeout = TimeSpan.FromSeconds(10);

        object reviewParam = new { };
        string reviewWhereCondition = GetWhereCondition(filterOptions, "r", "rt");
        string reviewSql =
@"select distinct count(r.id) as reviewsCount
    from [reviewing].Reviews        as r
    left join [reviewing].ReviewTag as rt on rt.ReviewId = r.Id
    " + reviewWhereCondition;
        IEnumerable<dynamic> reviewsCount = await connection.QueryAsync<dynamic>(reviewSql, reviewParam, commandTimeout: timeout.Seconds);

        return reviewsCount;
    }

    public async Task<dynamic> GetReviewsShortDescription(
        PaginationOptions paginationOptions, 
        ReviewSortOptions? sortOptions = null, 
        ReviewFilterOptions? filterOptions = null)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var timeout = TimeSpan.FromSeconds(10);

        object reviewParam = new { };
        string reviewWhereCondition = GetWhereCondition(filterOptions, "r", "rt");
        string reviewOrderCondition = GetOrderCondition(sortOptions, paginationOptions, "r", "lc");
        string reviewSql =
@"with likes_count (ReviewId, likesCount) as (
    select rl.ReviewId, count(rl.UserId)
        from [reviewing].ReviewLikes as rl
        group by rl.ReviewId
)
select distinct
         r.Id             as [id]
        ,r.[Name]         as [name]
        ,r.AuthorUserId   as [authorUserId]
        ,r.Content        as content
        ,r.[Status]       as [status]
        ,r.ImageUrl       as imageUrl
        ,r.Subject_Name   as subjectName
        ,r.Subject_Grade  as subjectGrage
        ,r.PublishedDate  as publishedDate
        ,sg.[Name]        as subjectGroupName
        ,lc.likesCount
    from [reviewing].Reviews        as r
    join [reviewing].SubjectGroups  as sg on sg.ReviewId = r.Id
    left join [reviewing].ReviewTag as rt on rt.ReviewId = r.Id
    left join likes_count           as lc on lc.ReviewId = r.Id
    " + reviewWhereCondition + " " + reviewOrderCondition;
        IEnumerable<ReviewVM> reviews = await connection.QueryAsync<ReviewVM>(reviewSql, reviewParam, commandTimeout: timeout.Seconds);
        if (reviews.Any())
        {
            string tagsSql =
@$"select 
         rt.TagsName as [name]
        ,rt.ReviewId as reviewId
    from [reviewing].ReviewTag as rt
    where rt.ReviewId in ({string.Join(", ", reviews.Select(r => $"'{r.id}'"))})";

            IEnumerable<TagVM> reviewsTags = await connection.QueryAsync<TagVM>(tagsSql, tagsSql, commandTimeout: timeout.Seconds);

            foreach (var review in reviews)
                review.tags = reviewsTags
                    .Where(t => t.reviewId == review.id)
                    .Select(t => t.name)
                    .ToList();
        }

        return reviews;
    }

    public async Task<dynamic> GetTags(string startWith = "")
    {
        var timeout = TimeSpan.FromSeconds(3); // TODO: set from config
        string whereCondition = string.IsNullOrWhiteSpace(startWith) ? "" : $"where t.[Name] like '{startWith}%'";
        string sql =
@$"select t.[Name] as [name]
    from [reviewing].[Tags] as t 
    " + whereCondition;

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        dynamic response = await connection.QueryAsync<dynamic>(sql, commandTimeout: timeout.Seconds);

        List<string> tags = new();
        foreach (dynamic item in response)
            tags.Add(item.name);

        return tags;
    }

    #region Query Builders
    string GetWhereCondition(ReviewFilterOptions? filterOptions, params object[] tableAlias)
    {
        string whereCondition = "";
        if (filterOptions is not null)
        {
            string where = "where ";
            List<string> conditions = new();

            if (!string.IsNullOrEmpty(filterOptions.Name))
            {
                string condition = $"{tableAlias[0]}.[Name] = {filterOptions.Name}";
                conditions.Add(condition);
            }

            if (!string.IsNullOrEmpty(filterOptions.Status))
            {
                string condition = $"{tableAlias[0]}.[Status] = {filterOptions.Status}";
                conditions.Add(condition);
            }

            if (!string.IsNullOrEmpty(filterOptions.SubjectName))
            {
                string condition = $"{tableAlias[0]}.[Subject_Name] = {filterOptions.SubjectName}";
                conditions.Add(condition);
            }

            if (filterOptions.Tags is not null && filterOptions.Tags.Count > 0)
            {
                string condition = $"{tableAlias[1]}.[TagsName] is null or {tableAlias[1]}.[TagsName] in ({string.Join(", ", filterOptions.Tags)})";
                conditions.Add(condition);
            }

            if (conditions.Count > 0)
                whereCondition = where + string.Join('\n', conditions);
        }

        return whereCondition;
    }

    string GetOrderCondition(ReviewSortOptions? sortOptions, PaginationOptions pagination, params object[] tableAlias)
    {
        string sortCondition = "";
        if (sortOptions is not null)
        {
            string tableFieldName = ReviewSortOptions.GetTableFieldName(sortOptions.SortField);
            string tableAs = sortOptions.SortField == SortFields.Likes ? tableAlias[1].ToString()! : tableAlias[0].ToString()!;
            sortCondition = $"order by {tableAs}.{tableFieldName} {sortOptions.SortType.GetDisplayName()}";
        }
        else
        {
            sortCondition = $"order by {tableAlias[0]}.PublishedDate desc";
        }

        int offset = pagination.PageNumber * pagination.PageSize;
        sortCondition += $"\n offset({offset}) rows fetch next({pagination.PageSize}) rows only";

        return sortCondition;
    }
    #endregion
}