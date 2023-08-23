using Reviewing.Domain.SeedWork.DomainExceptions;

namespace Reviewing.Domain.DomainExceptions;

public sealed class SomethingWentWrongDomainException
    : DomainException<SomethingWentWrongDomainException>
{
    static SomethingWentWrongDomainException()
    {
        MessageText = "Something went wrong";
    }

    public static void Throw(Exception? innerException = null)
    {
        ThrowEx(innerException: innerException);
    }

    public SomethingWentWrongDomainException(string message = "")
        : base(message)
    {
    }

    public SomethingWentWrongDomainException(string message = "", Exception? innerException = null)
        : base(message: message, innerException: innerException)
    {
    }
}
