using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Comments.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController 
        : ControllerBase
    {
        private readonly ICommentQueries commentQueries;

        public CommentController(ICommentQueries commentQueries)
        {
            this.commentQueries = commentQueries;
        }

        [HttpGet]
        public async Task<dynamic> GetReviewComments(string reviewId, int pageSize = 10, int pageNumber = 0)
        {
            return await commentQueries.GetReviewComments(reviewId, pageSize, pageNumber);
        }
    }
}
