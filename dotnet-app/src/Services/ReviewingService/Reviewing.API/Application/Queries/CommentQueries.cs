using Dapper;
using Microsoft.Data.SqlClient;

namespace Reviewing.API.Application.Queries;

public class CommentQueries
    : ICommentQueries
{
    private readonly string connectionString;

    public CommentQueries(
        string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<dynamic> GetReviewComments(string reviewId, int pageSize, int pageNumber)
    {
        using var connection = new SqlConnection(connectionString);
        int offset = pageNumber * pageSize;
        object param = new { offset, reviewId, pageSize };
        var timeout = TimeSpan.FromSeconds(10); // TODO: set from config
        string sql =
            @$"select
                     c.Id       as commentId
                    ,c.UserId   as userId
                    ,c.Text     as text
                    ,c.SentDate as sentDate
                from [reviewing].[Comments] as c
                where c.ReviewId = @{nameof(reviewId)}
                order by c.SentDate desc
                offset (@{nameof(offset)}) rows fetch next (@{nameof(pageSize)}) rows only";
        connection.Open();
        return await connection.QueryAsync<dynamic>(sql, param, commandTimeout: timeout.Seconds);
    }
}