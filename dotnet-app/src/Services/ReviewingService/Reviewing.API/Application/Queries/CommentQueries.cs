using Dapper;
using Microsoft.Data.SqlClient;

namespace Reviewing.API.Application.Queries;

public class CommentQueries
    : ICommentQueries
{
    private readonly string connectionString;

    public CommentQueries(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<dynamic> GetReviewComments(string reviewId, int pageSize, int pageNumber)
    {
        using var connection = new SqlConnection(connectionString);
        int offset = pageNumber * pageSize;
        connection.Open();
        return await connection.QueryAsync<dynamic>(
            @"select
                         c.Id       as commentId
                        ,c.UserId   as userId
                        ,c.Text     as text
                        ,c.SentDate as sentDate
                    from [dbo].[comments] as c
                    where c.ReviewId = @reviewId
                    order by c.SentDate desc
                    offset (@offset) rows fetch next (@pageSize) rows only",
            new { offset, reviewId, pageSize });
    }
}