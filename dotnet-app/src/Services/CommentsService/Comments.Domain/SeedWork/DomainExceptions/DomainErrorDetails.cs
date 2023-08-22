namespace Comments.Domain.SeedWork.DomainExceptions;

public class DomainErrorDetails
{
    public DomainErrorDetails(DomainException ex, string message)
    {
        Ex = ex;
        Message = message;
    }

    public DomainException Ex { get; init; }
    public string Message { get; init; }
}
