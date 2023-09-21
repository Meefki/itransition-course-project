using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Reviewing.Application.SeedWork;
using Reviewing.Domain.AggregateModels.CommentAggregate;
using Reviewing.Domain.AggregateModels.ReviewAggregate;
using Reviewing.Domain.Enumerations;
using Reviewing.Infrastructure.SeedWork;
using System.Data;

namespace Reviewing.Infrastructure;

public class ReviewingDbContext
    : DbContext, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "reviewing";

    private readonly IDomainEventMediator mediator = null!;
    private IDbContextTransaction? currentTransaction;

    public DbSet<Review> Reviews { get; set; }
    public DbSet<Comment> Comments { get; set; }

    public DbSet<Tag> Tags { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectGroups> SubjectGroups { get; set; }

    //public ReviewingDbContext(DbContextOptions<ReviewingDbContext> options) : base(options) { }
    public ReviewingDbContext(
        DbContextOptions<ReviewingDbContext> options,
        IDomainEventMediator mediator)
        : base(options)
    {
        this.mediator = mediator;
    }

    public IDbContextTransaction GetCurrentTransaction() => currentTransaction!;

    public bool HasActiveTransaction => currentTransaction != null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReviewingDbContext).Assembly);
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