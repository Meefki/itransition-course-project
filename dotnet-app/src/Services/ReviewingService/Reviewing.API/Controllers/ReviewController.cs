using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reviewing.API.Application.Commands.ReviewCommands;
using Reviewing.API.Controllers.ViewModels;

namespace Reviewing.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewController 
        : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IReviewQueries reviewQueries;

        public ReviewController(
            IMediator mediator,
            IReviewQueries reviewQueries)
        {
            this.mediator = mediator;
            this.reviewQueries = reviewQueries;
        }

        [HttpGet]
        [Route("tags")]
        public async Task<dynamic> GetTags(string? startWith = "")
        {
            return await reviewQueries.GetTags(startWith!);
        }

        [HttpPost]
        public async Task<dynamic> Publish([FromBody] ReviewVM vm)
        {
            PublishReviewCommand command = new(vm.Name, vm.AuthorUserId, vm.Content, vm.ImageUrl, vm.SubjectName, vm.SubjectGroupName, vm.Grade, vm.Tags);
            CommandResponse<string> response = await mediator.Send(command);

            return response;
        }

        [HttpPut]
        public async Task<CommandResponse> Edit([FromBody] ReviewVM vm)
        {
            EditReviewCommand command = new(vm.Id!, vm.Name, vm.Content, vm.ImageUrl, vm.SubjectName, vm.SubjectGroupName, vm.Grade, vm.Tags);
            CommandResponse response = await mediator.Send(command);

            return response;
        }

        [HttpDelete]
        public async Task<CommandResponse> Delete(string reviewId)
        {
            DeleteReviewCommand command = new(reviewId);
            CommandResponse response = await mediator.Send(command);

            return response;
        }
    }
}
