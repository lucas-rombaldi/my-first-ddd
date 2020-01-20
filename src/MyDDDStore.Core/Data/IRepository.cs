using MyDDDStore.Core.DomainObjects;
using System;

namespace MyDDDStore.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
