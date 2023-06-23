using Base.Contracts;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Base.Contracts.Domain;

namespace Base.BLL;

public class BaseEntityService<TBllEntity, TDalEntity, TRepository, TKey> : IEntityService<TBllEntity, TKey>
    where TBllEntity : class, IDomainEntityId<TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TRepository : IBaseRepository<TDalEntity, TKey>
    where TKey : struct, IEquatable<TKey>
{
    protected readonly TRepository Repository;
    protected readonly IMapper<TBllEntity, TDalEntity> Mapper;

    public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper)
    {
        Repository = repository;
        Mapper = mapper;
    }

    public virtual TBllEntity Add(TBllEntity entity)
    {
        return Mapper.Map(Repository.Add(Mapper.Map(entity)!))!;
    }

    public virtual TBllEntity Update(TBllEntity entity)
    {
        return Mapper.Map(Repository.Update(Mapper.Map(entity)!))!;
    }

    public virtual TBllEntity Remove(TBllEntity entity)
    {
        return Mapper.Map(Repository.Remove(Mapper.Map(entity)!))!;
    }

    public virtual TBllEntity Remove(TKey id)
    {
        return Mapper.Map(Repository.Remove(id))!;
    }
    
    public virtual TBllEntity? FirstOrDefault(TKey id)
    {
        return Mapper.Map(Repository.FirstOrDefault(id));
    }

    public virtual async Task<TBllEntity?> FirstOrDefaultAsync(TKey id)
    {
        return Mapper.Map(await Repository.FirstOrDefaultAsync(id));
    }

    public virtual async Task<IEnumerable<TBllEntity>> AllAsync()
    {
        return (await Repository.AllAsync()).Select(entity => Mapper.Map(entity))!;
    }
}

public class BaseEntityService<TBllEntity, TDalEntity, TRepository> : 
             BaseEntityService<TBllEntity, TDalEntity, TRepository, Guid>, IEntityService<TBllEntity>
    where TBllEntity : class, IDomainEntityId
    where TDalEntity : class, IDomainEntityId
    where TRepository : IBaseRepository<TDalEntity>
{
    public BaseEntityService(TRepository repository, IMapper<TBllEntity, TDalEntity> mapper) : base(repository, mapper)
    {
    }
}