using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reviewing.API.Application.Commands.ReviewCommands;
using Reviewing.API.Application.Queries.Options;

namespace Reviewing.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReviewsController 
        : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IReviewQueries reviewQueries;

        public ReviewsController(
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

        [HttpGet]
        [Route("{reviewId}")]
        public async Task<dynamic> GetReview(string reviewId)
        {
            return await reviewQueries.GetReview(reviewId);
        }

        [HttpGet]
        [Route("count")]
        public async Task<dynamic> GetReviewsCount([FromQuery] Dictionary<string, List<string>>? filterFields = null)
        {
            ReviewFilterOptions? filter = null;
            if (filterFields != null)
            {
                var lowerKeys = filterFields.Keys.ToDictionary(k => k.ToLower(), k => k);
                filter = new()
                {
                    Name = lowerKeys.ContainsKey("name") ? filterFields[lowerKeys["name"]].First().Trim() : null,
                    Status = lowerKeys.ContainsKey("status") ? filterFields[lowerKeys["status"]].First().Trim() : null,
                    SubjectName = lowerKeys.ContainsKey("subjectname") ? filterFields[lowerKeys["subjectname"]].First().Trim() : null,
                    Tags = lowerKeys.ContainsKey("tags") ? filterFields[lowerKeys["tags"]] : null
                };
            }

            return await reviewQueries.GetReviewsCount(filter);
        }

        [HttpGet]
        public async Task<dynamic> GetReviewShortDesriptions(
            int pageSize = 10,
            int pageNumber = 0,
            string? sortField = null,
            string? sortType = null, 
            [FromQuery] Dictionary<string, List<string>>? filterFields = null)
        {
            if (filterFields is not null)
            {
                if (filterFields.ContainsKey(nameof(pageSize))) filterFields.Remove(nameof(pageSize));
                if (filterFields.ContainsKey(nameof(pageNumber))) filterFields.Remove(nameof(pageNumber));
                if (filterFields.ContainsKey(nameof(sortField))) filterFields.Remove(nameof(sortField));
                if (filterFields.ContainsKey(nameof(sortType))) filterFields.Remove(nameof(sortType));
            }

            PaginationOptions pagination = new(pageSize, pageNumber);
            ReviewSortOptions sort = new()
            {
                SortField = ReviewSortOptions.MapStringToSortField(sortField?.ToLower() ?? ""),
                SortType = ReviewSortOptions.MapStringToSortType(sortType?.ToLower() ?? "")
            };
            ReviewFilterOptions? filter = null;
            if (filterFields != null)
            {
                var lowerKeys = filterFields.Keys.ToDictionary(k => k.ToLower(), k => k);
                filter = new()
                {
                    Name = lowerKeys.ContainsKey("name") ? filterFields[lowerKeys["name"]].First().Trim() : null,
                    Status = lowerKeys.ContainsKey("status") ? filterFields[lowerKeys["status"]].First().Trim() : null,
                    SubjectName = lowerKeys.ContainsKey("subjectname") ? filterFields[lowerKeys["subjectname"]].First().Trim() : null,
                    Tags = lowerKeys.ContainsKey("tags") ? filterFields[lowerKeys["tags"]] : null
                };
            }

            return await reviewQueries.GetReviewsShortDescription(pagination, sort, filter);
        }

        [HttpPost]
        [Authorize]
        public async Task<dynamic> Publish([FromBody] ReviewOptions opt)
        {
            PublishReviewCommand command = new(opt.Name, opt.AuthorUserId, opt.Content, opt.ShortDesc, opt.ImageUrl, opt.SubjectName, opt.SubjectGroupName, opt.SubjectGrade, opt.Tags);
            CommandResponse<string> response = await mediator.Send(command);

            return response;
        }

        [HttpPut]
        public async Task<CommandResponse> Edit([FromBody] ReviewOptions opt)
        {
            EditReviewCommand command = new(opt.Id!, opt.Name, opt.Content, opt.ShortDesc, opt.ImageUrl, opt.SubjectName, opt.SubjectGroupName, opt.SubjectGrade, opt.Tags);
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
