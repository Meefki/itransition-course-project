namespace Comments.Application.Commands;

public abstract record AddCommentAbstractCommand(string ReviewId, string UserId, string Text);