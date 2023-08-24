using Microsoft.AspNetCore.Mvc;

namespace Reviewing.API.Controllers
{
    [Route("api/v1/[controller]")]
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
            return await commentQueries.GetReviewComments(reviewId, new(pageSize, pageNumber));
        }
    }
}
