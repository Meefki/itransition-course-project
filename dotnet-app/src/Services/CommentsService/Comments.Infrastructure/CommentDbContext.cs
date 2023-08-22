using Comments.Domain;
using Comments.Domain.SeedWork;
using Comments.Infrastructure.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using Users.Application.SeedWork.Mediator;

namespace Comments.Infrastructure;

public class CommentDbContext
    : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "dbo";

    private readonly IDomainEventMediator mediator = null!;
    private IDbContextTransaction? currentTransaction;

    public DbSet<Comment> Comments { get; set; }

    public CommentDbContext(DbContextOptions<CommentDbContext> options) : base(options) { }
    public CommentDbContext(
        DbContextOptions<CommentDbContext> options,
        IDomainEventMediator mediator)
        : base(options)
    {
        this.mediator = mediator;
    }

    public IDbContextTransaction GetCurrentTransaction() => currentTransaction!;

    public bool HasActiveTransaction => currentTransaction != null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CommentDbContext).Assembly);
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEventsAsync(this, cancellationToken);

        await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (currentTransaction != null) return null;

        currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

        return currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (transaction != currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

        try
        {
            await SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (currentTransaction != null)
            {
                currentTransaction.Dispose();
                currentTransaction = null;
            }
        }
    }

    public void RollbackTransaction()
    {
        try
        {
            currentTransaction?.Rollback();
        }
        finally
        {
            if (currentTransaction != null)
            {
                currentTransaction.Dispose();
                currentTransaction = null;
            }
        }
    }
}