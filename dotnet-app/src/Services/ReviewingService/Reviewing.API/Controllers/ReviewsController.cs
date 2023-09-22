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
        [Route("tags/popular")]
        public async Task<dynamic> GetMostPopularTags(int? pageSize = 10)
        {
            return await reviewQueries.GetMostPopularTags(pageSize!.Value);
        }

        [HttpGet]
        [Route("subjects")]
        public async Task<dynamic> GetSubjects(string? startWith = null)
        {
            return await reviewQueries.GetSubjects(startWith ?? "");
        }

        [HttpGet]
        [Route("{reviewId}")]
        public async Task<dynamic> GetReview(
            string reviewId,
            string? userId = null)
        {
            dynamic review = await reviewQueries.GetReview(reviewId, userId);
            return review;
        }

        [HttpGet]
        [Route("count")]
        public async Task<dynamic> GetReviewsCount([FromQuery] Dictionary<string, string>? filterOptions = null, [FromQuery] List<string>? tags = null)
        {
            ReviewFilterOptions? filter = null;
            if (filterOptions != null)
            {
                var lowerKeys = filterOptions.Keys.ToDictionary(k => k.ToLower(), k => k);
                filter = new()
                {
                    Name = lowerKeys.ContainsKey("name") ? filterOptions[lowerKeys["name"]].Trim() : null,
                    AuthorUserId = lowerKeys.ContainsKey("authoruserid") ? filterOptions[lowerKeys["authoruserid"]].Trim() : null,
                    Content = lowerKeys.ContainsKey("content") ? filterOptions[lowerKeys["content"]].Trim() : null,
                    Status = lowerKeys.ContainsKey("status") ? filterOptions[lowerKeys["status"]].Trim() : null,
                    SubjectName = lowerKeys.ContainsKey("subjectname") ? filterOptions[lowerKeys["subjectname"]].Trim() : null,
                    Tags = tags
                };
            }

            return await reviewQueries.GetReviewsCount(filter);
        }

        [HttpGet]
        public async Task<dynamic> GetReviewShortDesriptions(
            int pageSize = 10,
            int pageNumber = 0,
            [FromQuery] Dictionary<string, string>? sortOptions = null,
            [FromQuery] Dictionary<string, string>? filterOptions = null,
            [FromQuery] List<string>? tags = null,
            string? userId = null)
        {
            if (filterOptions is not null)
            {
                if (filterOptions.ContainsKey(nameof(pageSize))) filterOptions.Remove(nameof(pageSize));
                if (filterOptions.ContainsKey(nameof(pageNumber))) filterOptions.Remove(nameof(pageNumber));
                if (filterOptions.ContainsKey(nameof(sortOptions))) filterOptions.Remove(nameof(sortOptions));
            }

            if (sortOptions is not null)
            {
                if (sortOptions.ContainsKey(nameof(pageSize))) sortOptions.Remove(nameof(pageSize));
                if (sortOptions.ContainsKey(nameof(pageNumber))) sortOptions.Remove(nameof(pageNumber));
            }

            PaginationOptions pagination = new(pageSize, pageNumber);
            List<ReviewSortOptions> sort = new(sortOptions?.Select(x => new ReviewSortOptions()
            {
                SortField = ReviewSortOptions.MapStringToSortField(x.Key.ToLower()),
                SortType = ReviewSortOptions.MapStringToSortType(x.Value?.ToLower() ?? string.Empty)
            }) ?? new List<ReviewSortOptions>());
            ReviewFilterOptions? filter = null;
            if (filterOptions != null)
            {
                var lowerKeys = filterOptions.Keys.ToDictionary(k => k.ToLower(), k => k);
                filter = new()
                {
                    Name = lowerKeys.ContainsKey("name") ? filterOptions[lowerKeys["name"]].Trim() : null,
                    AuthorUserId = lowerKeys.ContainsKey("authoruserid") ? filterOptions[lowerKeys["authoruserid"]].Trim() : null,
                    Content = lowerKeys.ContainsKey("content") ? filterOptions[lowerKeys["content"]].Trim() : null,
                    Status = lowerKeys.ContainsKey("status") ? filterOptions[lowerKeys["status"]].Trim() : null,
                    SubjectName = lowerKeys.ContainsKey("subjectname") ? filterOptions[lowerKeys["subjectname"]].Trim() : null,
                    Tags = tags
                };
            }

            return await reviewQueries.GetReviewsShortDescription(pagination, sort, filter, userId);
        }

        [HttpPost]
        [Authorize]
        [Authorize(Policy = "Review_edit")]
        public async Task<dynamic> Publish([FromBody] ReviewOptions opt)
        {
            string? imageString = string.IsNullOrEmpty(opt.image) ? null : opt.image[(opt.image.IndexOf("base64,") + "base64,".Length)..];
            var bytes = string.IsNullOrEmpty(imageString) ? null : Convert.FromBase64String(imageString);
            Stream? imageStream = bytes is null ? null : new MemoryStream(bytes);

            PublishReviewCommand command = new(opt.Name, opt.AuthorUserId, opt.Content, opt.ShortDesc, opt.imageType, imageStream, opt.SubjectId, opt.SubjectName, opt.SubjectGrade, opt.Tags);
            CommandResponse<string> response = await mediator.Send(command);

            return response;
        }

        [HttpPut]
        [Authorize]
        public async Task<CommandResponse> Edit([FromBody] ReviewOptions opt)
        {
            string? imageString = string.IsNullOrEmpty(opt.image) ? null : opt.image[(opt.image.IndexOf("base64,") + "base64,".Length)..];
            var bytes = string.IsNullOrEmpty(imageString) ? null : Convert.FromBase64String(imageString);
            Stream? imageStream = bytes is null ? null : new MemoryStream(bytes);

            EditReviewCommand command = new(opt.Id!, opt.Name, opt.Content, opt.ShortDesc, opt.imageType, imageStream, opt.SubjectId, opt.SubjectName, opt.SubjectGrade, opt.Tags);
            CommandResponse response = await mediator.Send(command);

            return response;
        }

        [HttpDelete]
        [Authorize(Policy = "Review_edit")]
        public async Task<CommandResponse> Delete(string reviewId)
        {
            DeleteReviewCommand command = new(reviewId);
            CommandResponse response = await mediator.Send(command);

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("like")]
        public async Task<CommandResponse> Like(string reviewId, string userId)
        {
            LikeReviewCommand command = new(reviewId, userId);
            CommandResponse response = await mediator.Send(command);

            return response;
        }

        [HttpPost]
        [Authorize]
        [Route("estimate")]
        public async Task<CommandResponse> Estimate(string reviewId, string userId, int grade)
        {
            EstimateReviewCommand command = new(reviewId, userId, grade);
            CommandResponse response = await mediator.Send(command);

            return response;
        }
    }
}
