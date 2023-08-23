using Reviewing.Domain.Enumerations;
using Reviewing.Domain.SeedWork.DomainExceptions;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate.DomainExceptions;

public class ConnotChangeReviewStatusDomainException
    : DomainException<ConnotChangeReviewStatusDomainException>
{
    static ConnotChangeReviewStatusDomainException()
    {
        MessageText = "Cannot change review status from {0} to {1}";
    }

    public static void Throw(ReviewStatuses from, ReviewStatuses to)
    {
        ThrowEx(string.Format(MessageText, from, to));
    }

    public ConnotChangeReviewStatusDomainException(string message = "")
        : base(message)
    {
    }

    public ConnotChangeReviewStatusDomainException(string message = "", Exception? innerException = null)
        : base(message: message, innerException: innerException)
    {
    }
}