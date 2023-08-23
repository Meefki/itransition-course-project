using Comments.Application.Commands;
using Comments.Application.SeedWork;
using MediatR;

namespace Comments.API.Application.Commands;

public record RemoveCommentCommand(string CommentId) : 
    RemoveCommentAbstractCommand(CommentId), 
    IRequest<CommandResponse>;
