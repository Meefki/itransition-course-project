using Comments.Domain.DomainExceptions;
using Comments.Domain.SeedWork.DomainExceptions;
using Microsoft.Extensions.Logging;

namespace Comments.Application.SeedWork;

public abstract class CommandHandler<TRequest, TResponse>
    where TRequest : class
    where TResponse : class?
{
    private readonly ILogger logger;

    public CommandHandler(ILogger logger)
    {
        this.logger = logger;
    }

    protected abstract Task<TResponse> Action(TRequest request, CancellationToken cancellationToken);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
    public async Task<CommandResponse<TResponse>> Handle(TRequest request, CancellationToken cancellationToken)
    {
        DomainErrorDetails? errorDetails = null;
        bool result = true;
        TResponse? objectResult = null;

        try
        {
            objectResult = await Action(request, cancellationToken);
        }
        catch (DomainException ex)
        {
            errorDetails = new(ex, ex.Message);
            result = false;
            logger.LogError(ex, message: ex.Message);
        }
        catch (Exception ex)
        {
            SomethingWentWrongDomainException domainException = new(SomethingWentWrongDomainException.MessageText, ex);
            errorDetails = new(domainException, SomethingWentWrongDomainException.MessageText);
            result = false;
            logger.LogError(domainException, message: SomethingWentWrongDomainException.MessageText);
            logger.LogCritical(ex, message: ex.Message);
        }

        CommandResponse<TResponse> response =
            new(objectResult,
                result,
                errorDetails);

        return response;
    }
}

public abstract class CommandHandler<TRequest>
{
    private readonly ILogger logger;

    public CommandHandler(ILogger logger)
    {
        this.logger = logger;
    }

    protected abstract Task Action(TRequest request, CancellationToken cancellationToken);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
    public async Task<CommandResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        DomainErrorDetails? errorDetails = null;
        bool result = true;

        try
        {
            await Action(request, cancellationToken);
        }
        catch (DomainException ex)
        {
            errorDetails = new(ex, ex.Message);
            result = false;
            logger.LogError(ex, message: ex.Message);
        }
        catch (Exception ex)
        {
            SomethingWentWrongDomainException domainException = new(SomethingWentWrongDomainException.MessageText, ex);
            errorDetails = new(domainException, SomethingWentWrongDomainException.MessageText);
            result = false;
            logger.LogError(domainException, message: SomethingWentWrongDomainException.MessageText);
            logger.LogCritical(ex, message: ex.Message);
        }

        CommandResponse response =
            new(result,
                errorDetails);

        return response;
    }
}

public abstract class NotificationHandler<TRequest>
{
    private readonly ILogger logger;

    public NotificationHandler(ILogger logger)
    {
        this.logger = logger;
    }

    protected abstract Task Action(TRequest request, CancellationToken cancellationToken);

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
    public async Task Handle(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await Action(request, cancellationToken);
        }
        catch (DomainException ex)
        {
            logger.LogError(ex, message: ex.Message);
        }
        catch (Exception ex)
        {
            SomethingWentWrongDomainException domainException = new(SomethingWentWrongDomainException.MessageText, ex);
            logger.LogError(domainException, message: SomethingWentWrongDomainException.MessageText);
            logger.LogCritical(ex, message: ex.Message);
        }
    }
}
