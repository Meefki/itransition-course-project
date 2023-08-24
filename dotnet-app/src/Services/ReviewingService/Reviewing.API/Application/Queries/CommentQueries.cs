using Dapper;
using Microsoft.Data.SqlClient;
using Reviewing.API.Application.Queries.Options;
using Reviewing.API.Application.Queries.ViewModels;

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

    public async Task<dynamic> GetReviewComments(string reviewId, PaginationOptions? pagination = null)
    {
        object param;
        string paginationCondition = "";
        if (pagination is not null)
        {
            int offset = pagination.PageNumber * pagination.PageSize;
            param = new { reviewId, offset, pagination.PageSize };
            paginationCondition = @" offset(@offset) rows fetch next(@pageSize) rows only";
        }
        else
        {
            param = new { reviewId };
        }
        var timeout = TimeSpan.FromSeconds(10); // TODO: set from config
        string sql =
            @"select
                     c.Id       as commentId
                    ,c.UserId   as userId
                    ,c.Text     as text
                    ,c.SentDate as sentDate
                from [reviewing].[Comments] as c
                where c.ReviewId = @reviewId
                order by c.SentDate desc 
                " + paginationCondition;
        using var connection = new SqlConnection(connectionString);
        connection.Open();
        dynamic comments = await connection.QueryAsync<dynamic>(sql, param, commandTimeout: timeout.Seconds);

        return comments;

        //List<CommentVM> commentsVM = new();
        //foreach (var comment in comments)
        //    commentsVM.Add(MapDynamicToComment(comment));

        //return commentsVM;
    }

    private static CommentVM MapDynamicToComment(dynamic dynamicComment)
    {
        CommentVM commentVM = new()
        {
            CommentId = dynamicComment.commentId,
            UserId = dynamicComment.userId,
            SentDate = dynamicComment.sentDate,
            Text = dynamicComment.text,
        };

        return commentVM;
    }
}