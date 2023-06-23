using Base.Contracts.Domain;

namespace Base.Contracts.DAL;

public interface IBaseRepository<TEntity, in TKey>
    where TEntity : class, IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
{
    TEntity Add(TEntity entity);
    TEntity Update(TEntity entity);
    TEntity Remove(TEntity entity);
    TEntity Remove(TKey entity);
    TEntity? FirstOrDefault(TKey id);
    
    Task<TEntity?> FirstOrDefaultAsync(TKey id);
    Task<IEnumerable<TEntity>> AllAsync();
}

public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, Guid>
    where TEntity : class, IDomainEntityId
{
}