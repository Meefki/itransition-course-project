namespace Reviewing.Application.Commands.CommentCommands;

public abstract record AddCommentAbstractCommand(string ReviewId, string UserId, string Text);