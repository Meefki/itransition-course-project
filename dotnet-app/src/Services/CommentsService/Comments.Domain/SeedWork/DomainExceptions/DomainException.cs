namespace Comments.Domain.SeedWork.DomainExceptions;

public abstract class DomainException<T>
    : DomainException
    where T : DomainException<T>
{
    public static string MessageText { get; protected set; } = "";

    protected static void ThrowEx(string message = "", Exception? innerException = null)
    {
        object[] objectParams = innerException is null ?
            new object[] { message } :
            new object[] { message, innerException };

        throw (Activator.CreateInstance(typeof(T), objectParams) as T)!;
    }

    public DomainException(string message = "")
        : base(message)
    {
    }

    public DomainException(string message = "", Exception? innerException = null)
        : base(message: message, innerException: innerException)
    {
    }
}

public abstract class DomainException
    : Exception
{
    public DomainException(string message = "")
        : base(message)
    {
    }

    public DomainException(string message = "", Exception? innerException = null)
        : base(message: message, innerException: innerException)
    {
    }
}
