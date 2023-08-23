using Reviewing.Domain.SeedWork.DomainExceptions;

namespace Reviewing.Domain.AggregateModels.ReviewAggregate.DomainExceptions;

public class WrongSubjectGradeDomainException
    : DomainException<WrongSubjectGradeDomainException>
{
    static WrongSubjectGradeDomainException()
    {
        MessageText = "Grade value must be between 1 and 10";
    }

    public static void Throw()
    {
        ThrowEx();
    }

    public WrongSubjectGradeDomainException(string message = "")
        : base(message)
    {
    }

    public WrongSubjectGradeDomainException(string message = "", Exception? innerException = null)
        : base(message: message, innerException: innerException)
    {
    }
}