using Reviewing.Domain.SeedWork;

namespace Reviewing.Application.SeedWork;

public interface IRepository<T>
    where T : IAggregateRoot
{
    IUnitOfWork UnitOfWork { get; }
}
