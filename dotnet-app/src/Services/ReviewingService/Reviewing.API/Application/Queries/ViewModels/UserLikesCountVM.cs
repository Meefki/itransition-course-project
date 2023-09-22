namespace Reviewing.API.Application.Queries.ViewModels;

public record UserLikesCountVM(Guid userId, int likesCount);