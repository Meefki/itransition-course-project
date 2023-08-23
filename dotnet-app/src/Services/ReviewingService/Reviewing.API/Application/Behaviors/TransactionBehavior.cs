using Microsoft.EntityFrameworkCore;
using MediatR;
using Reviewing.Infrastructure;
using Reviewing.API.Extentions;

namespace Reviewing.API.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> logger;
    private readonly ReviewingDbContext context;

    public TransactionBehavior(ReviewingDbContext context,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        this.context = context ?? throw new ArgumentException(nameof(ReviewingDbContext));
        this.logger = logger ?? throw new ArgumentException(nameof(ILogger));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = default(TResponse);
        var typeName = request.GetGenericTypeName();

        try
        {
            if (context.HasActiveTransaction)
            {
                return await next();
            }

            var strategy = context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                Guid transactionId;

                await using var transaction = await context.BeginTransactionAsync();
                using (logger.BeginScope(new List<KeyValuePair<string, object>> { new("TransactionContext", transaction!.TransactionId) }))
                {
                    logger.LogInformation("Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, typeName, request);

                    response = await next();

                    logger.LogInformation("Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, typeName);

                    await context.CommitTransactionAsync(transaction);

                    transactionId = transaction.TransactionId;
                }
            });

            return response!;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Handling transaction for {CommandName} ({@Command})", typeName, request);

            throw;
        }
    }
}