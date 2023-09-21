using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Extensions;
using Reviewing.API.Application.Queries.Options;
using Reviewing.API.Application.Queries.ViewModels;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Identifiers;

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

    public async Task<dynamic> GetReview(
        string reviewId, 
        string? userId = null)
    {
        var timeout = TimeSpan.FromSeconds(10);
        object param = new { reviewId };
        string reviewSql =
@"with likes_count (likesCount) as (
    select count(rl.UserId)
        from [reviewing].ReviewLikes as rl
        where rl.ReviewId = @reviewId
),
estimate (estimate) as (
  select cast(iif(count(e.Grade) = 0, 0, cast(sum(e.Grade) as float) / cast(count(e.Grade) as float)) as numeric(10,1))
    from [reviewing].Estimates as e
    where e.ReviewId = @reviewId
)
select top 1
         r.Id             as [id]
        ,r.[Name]         as [name]
        ,r.AuthorUserId   as [authorUserId]
        ,r.Content        as content
        ,r.[Status]       as [status]
        ,r.ImageUrl       as imageUrl
        ,r.PublishedDate  as publishedDate
        ,s.[Name]         as subjectName
        ,s.[Grade]        as subjectGrade
        ,sg.[Name]        as subjectGroupName
        ,lc.likesCount
        ,e.estimate
    from [reviewing].Reviews          as r
    join [reviewing].[Subjects]       as s  on s.Id  = r.SubjectId
    join [reviewing].[SubjectGroups]  as sg on sg.Id = s.GroupId
    cross join likes_count            as lc
    cross join estimate               as e
    where r.Id = @reviewId";
        string tagsSql =
@"select rt.TagsName as [name]
    from [reviewing].ReviewTag as rt
    where rt.ReviewId = @reviewId";

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        ReviewVM review = (await connection.QueryAsync<ReviewVM>(reviewSql, param, commandTimeout: timeout.Seconds)).Single();
        dynamic reviewTags = await connection.QueryAsync<dynamic>(tagsSql, param, commandTimeout: timeout.Seconds);

        List<string> tags = new();
        foreach (dynamic tag in reviewTags)
            tags.Add(tag.name);
        review.tags = tags;

        if (!string.IsNullOrEmpty(userId))
        {
            param = new { reviewId, userId };
            string estimateSql =
@"select
     e.Grade                                   as estimate
    ,cast(iif(rl.UserId is null, 0, 1) as bit) as isLiked
  from [reviewing].[reviewing].[Reviews]          as r
  left join [reviewing].[reviewing].[Estimates]   as e  on e.ReviewId = r.Id
  left join [reviewing].[reviewing].[ReviewLikes] as rl on rl.ReviewId = r.Id and rl.UserId = @userId
  where r.Id = @reviewId";

            dynamic? estimate = (await connection.QueryAsync<dynamic>(estimateSql, param, commandTimeout: timeout.Seconds)).FirstOrDefault();
            review.isLiked = estimate?.isLiked ?? false;
            review.userEstimate = estimate?.estimate ?? 0;
        }

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
@"select count(t.Id) as reviewsCount
  from (
    select distinct r.Id
      from [reviewing].[Reviews]        as r
      " + ((filterOptions?.Tags is not null && filterOptions.Tags.Count > 0) ? @"left join [reviewing].[ReviewTag] as rt on rt.ReviewId = r.Id
      " : "") + reviewWhereCondition + @"
    ) as t";
        IEnumerable<dynamic> reviewsCount = await connection.QueryAsync<dynamic>(reviewSql, reviewParam, commandTimeout: timeout.Seconds);

        return reviewsCount;
    }

    public async Task<dynamic> GetSubjects(string startWith)
    {
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        var timeout = TimeSpan.FromSeconds(10);

        object reviewParam = new { };
        string subjectsSql =
@$"select 
     s.Id     as [id]
    ,s.[Name] as [name]
  from [reviewing].[Subjects] as s
  {(string.IsNullOrEmpty(startWith) ? "" : $"where s.[name] like '{startWith}%'" )}";
        IEnumerable<dynamic> subjects = await connection.QueryAsync<dynamic>(subjectsSql, reviewParam, commandTimeout: timeout.Seconds);

        return subjects;
    }

    public Task<dynamic> GetSubjectGroups()
    {
        throw new NotImplementedException();
    }

    public async Task<dynamic> GetReviewsShortDescription(
        PaginationOptions paginationOptions, 
        List<ReviewSortOptions>? sortOptions = null,
        ReviewFilterOptions? filterOptions = null,
        string? userId = null)
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
),
estimate (ReviewId, estimate) as (
  select e.ReviewId, cast(iif(count(e.Grade) = 0, 0, cast(sum(e.Grade) as float) / cast(count(e.Grade) as float)) as numeric(10,1))
    from [reviewing].Estimates as e
    group by e.ReviewId
)
select distinct
         r.Id             as [id]
        ,r.[Name]         as [name]
        ,r.AuthorUserId   as [authorUserId]
        ,r.ShortDesc      as shortDesc
        ,r.[Status]       as [status]
        ,r.ImageUrl       as imageUrl
        ,r.PublishedDate  as publishedDate
        ,lc.likesCount
        ,e.estimate
    from [reviewing].[Reviews]        as r
    join [reviewing].[Subjects]       as s  on s.Id        = r.SubjectId
    join [reviewing].[SubjectGroups]  as sg on sg.Id       = s.GroupId
    left join [reviewing].[ReviewTag] as rt on rt.ReviewId = r.Id
    left join likes_count             as lc on lc.ReviewId = r.Id
    left join estimate                as e  on e.ReviewId  = r.Id
    " + reviewWhereCondition + "\n    " + reviewOrderCondition;
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
            {
                review.tags = reviewsTags
                    .Where(t => t.reviewId == review.id)
                    .Select(t => t.name)
                    .ToList();
            }
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

        //List<string> tags = new();
        //foreach (dynamic item in response)
        //    tags.Add(item.name);

        return response;
    }

    public async Task<dynamic> GetMostPopularTags(int pageSize = 10)
    {
        var timeout = TimeSpan.FromSeconds(3); // TODO: set from config
        string sql =
@$"select top {pageSize} 
     t.[Name]  as [value]
    ,t.[Count] as [count]
  from (
  select 
       t.[Name]
      ,count(rt.ReviewId) as [Count]
    from [reviewing].[reviewing].[Tags]           as t
    left join [reviewing].[reviewing].[ReviewTag] as rt on rt.TagsName = t.[Name]
    group by t.[Name]
  ) as t
  order by t.[Count] desc";

        using var connection = new SqlConnection(connectionString);
        connection.Open();
        dynamic tags = await connection.QueryAsync<dynamic>(sql, commandTimeout: timeout.Seconds);

        //List<dynamic> tags = new();
        //foreach (dynamic item in response)
        //    tags.Add(item.name);

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
                string condition = $"{tableAlias[0]}.[Name] = '{filterOptions.Name}'";
                conditions.Add(condition);
            }

            if (!string.IsNullOrEmpty(filterOptions.Content))
            {
                string condition = $"{tableAlias[0]}.[Content] = '{filterOptions.Content}'";
                conditions.Add(condition);
            }

            if (!string.IsNullOrEmpty(filterOptions.AuthorUserId))
            {
                string condition = $"{tableAlias[0]}.[AuthorUserId] = '{filterOptions.AuthorUserId}'";
                conditions.Add(condition);
            }

            if (!string.IsNullOrEmpty(filterOptions.Status))
            {
                string condition = $"{tableAlias[0]}.[Status] = '{filterOptions.Status}'";
                conditions.Add(condition);
            }

            if (!string.IsNullOrEmpty(filterOptions.SubjectName))
            {
                string condition = $"{tableAlias[0]}.[Subject_Name] = '{filterOptions.SubjectName}'";
                conditions.Add(condition);
            }

            if (filterOptions.Tags is not null && filterOptions.Tags.Count > 0)
            {
                string condition = $"{tableAlias[1]}.[TagsName] in ({string.Join(", ", filterOptions.Tags.Select(x => $"'{x}'"))})";
                conditions.Add(condition);
            }

            if (conditions.Count > 0)
                whereCondition = where + string.Join("\n      and ", conditions);
        }

        return whereCondition;
    }

    string GetOrderCondition(List<ReviewSortOptions>? sortOptions, PaginationOptions pagination, params object[] tableAlias)
    {
        string sortCondition = "";
        if (sortOptions is not null && sortOptions.Any())
        {
            string orderBy = "order by ";
            foreach (ReviewSortOptions sortOption in sortOptions)
            {
                string tableFieldName = ReviewSortOptions.GetTableFieldName(sortOption.SortField);
                string tableAs = sortOption.SortField == SortFields.Likes ? tableAlias[1].ToString()! : tableAlias[0].ToString()!;
                sortCondition += string.IsNullOrEmpty(sortCondition) ? "" : ", ";
                sortCondition += $"{tableAs}.{tableFieldName} {sortOption.SortType.GetDisplayName()} ";
            }
            sortCondition = string.IsNullOrEmpty(sortCondition) ? "" : (orderBy + sortCondition);
        }
        else
        {
            sortCondition = $"order by {tableAlias[0]}.PublishedDate desc";
        }

        int offset = pagination.PageNumber * pagination.PageSize;
        sortCondition += $"\n    offset({offset}) rows fetch next({pagination.PageSize}) rows only";

        return sortCondition;
    }
    #endregion
}