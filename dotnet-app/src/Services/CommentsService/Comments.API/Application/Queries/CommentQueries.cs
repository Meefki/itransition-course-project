using Dapper;
using Microsoft.Data.SqlClient;

namespace Comments.API.Application.Queries;

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
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            return await connection.QueryAsync<dynamic>(
                @"select 
                         c.Id       as commentId
                        ,c.UserId   as userId
                        ,c.Text     as text
                    from [dbo].[comments] as c
                    where c.ReviewId = @reviewId",
                new { reviewId });
        }
    }
}