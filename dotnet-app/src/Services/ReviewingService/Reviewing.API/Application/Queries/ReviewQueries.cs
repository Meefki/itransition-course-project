using Dapper;
using Microsoft.Data.SqlClient;
using Reviewing.API.Application.Queries.Options;

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

    public Task<dynamic> GetReview(string reviewId)
    {
        throw new NotImplementedException();
    }

    public Task<dynamic> GetReviewsDescription(PaginationOptions paginationOptions, ReviewSortOptions sortOptions, ReviewFilterOptions filterOptions)
    {
        throw new NotImplementedException();
    }

    public async Task<dynamic> GetTags(string startWith = "")
    {
        using var connection = new SqlConnection(connectionString);
        var timeout = TimeSpan.FromSeconds(3); // TODO: set from config
        string whereCondition = string.IsNullOrWhiteSpace(startWith) ? "" : $"where t.[Name] like '{startWith}%'";
        string sql =
            @$"select t.[Name]
                  from [reviewing].[Tags] as t" + whereCondition;
        connection.Open();
        dynamic response = await connection.QueryAsync<dynamic>(sql, commandTimeout: timeout.Seconds);

        List<string> tags = new();
        foreach (dynamic item in response)
            tags.Add(item.Name);

        return tags;
    }
}