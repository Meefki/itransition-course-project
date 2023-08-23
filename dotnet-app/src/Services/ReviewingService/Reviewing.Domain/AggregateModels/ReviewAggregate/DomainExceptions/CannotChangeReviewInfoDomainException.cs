using Reviewing.Domain.SeedWork.DomainExceptions;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate.DomainExceptions;

public class CannotChangeReviewInfoDomainException
    : DomainException<CannotChangeReviewInfoDomainException>
{
    static CannotChangeReviewInfoDomainException()
    {
        MessageText = "Cannot change review info after publication";
    }

    public static void Throw()
    {
        ThrowEx();
    }

    public CannotChangeReviewInfoDomainException(string message = "")
        : base(message)
    {
    }

    public CannotChangeReviewInfoDomainException(string message = "", Exception? innerException = null)
        : base(message: message, innerException: innerException)
    {
    }
}