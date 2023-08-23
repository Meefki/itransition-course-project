using Comments.API.Application.Commands;
using Comments.Application.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Comments.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IMediator mediator;

        public TestController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<CommandResponse> AddComment(string reviewId, string userId, string text)
        {
            AddCommentCommand command = new(reviewId, userId, text);
            var response = await mediator.Send(command);

            return response;
        }

        [HttpDelete]
        public async Task<CommandResponse> DeleteComment(string commentId)
        {
            RemoveCommentCommand command = new(commentId);
            var response = await mediator.Send(command);

            return response;
        }
    }
}
