using Comments.Domain.SeedWork.DomainExceptions;

namespace Comments.Application.SeedWork;

public class CommandResponse<T>
    : CommandResponse
{
    public CommandResponse(T? objectResult, bool result, DomainErrorDetails? errorDetails)
        : base(result, errorDetails)
    {
        ObjectResult = objectResult;
    }

    public T? ObjectResult { get; init; }
}

public class CommandResponse
{
    public CommandResponse(bool result, DomainErrorDetails? errorDetails)
    {
        Result = result;
        ErrorDetails = errorDetails;
    }

    public DomainErrorDetails? ErrorDetails { get; init; }
    public bool Result { get; init; }
}
