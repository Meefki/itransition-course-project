using System.Text.Json.Serialization;

namespace Reviewing.Domain.SeedWork.DomainExceptions;

public class DomainErrorDetails
{
    public DomainErrorDetails(DomainException ex, string message)
    {
        Ex = ex;
        Message = message;
    }


    [JsonIgnore]
    public DomainException Ex { get; init; }
    public string Message { get; init; }
}
