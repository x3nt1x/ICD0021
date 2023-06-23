using Base.Contracts;
using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseEntityRepository<TDalEntity, TDomainEntity, TKey, TDbContext> : IBaseRepository<TDalEntity, TKey>
    where TDalEntity : class, IDomainEntityId<TKey>
    where TDomainEntity : class, IDomainEntityId<TKey>
    where TKey : struct, IEquatable<TKey>
    where TDbContext : DbContext
{
    protected readonly TDbContext RepositoryDbContext;
    protected readonly DbSet<TDomainEntity> RepositoryDbSet;
    protected readonly IMapper<TDalEntity, TDomainEntity> RepoMapper;

    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity, TDomainEntity> mapper)
    {
        RepoMapper = mapper;
        RepositoryDbContext = dbContext;
        RepositoryDbSet = dbContext.Set<TDomainEntity>();
    }

    public virtual TDalEntity Add(TDalEntity entity)
    {
        return RepoMapper.Map(RepositoryDbSet.Add(RepoMapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        return RepoMapper.Map(RepositoryDbSet.Update(RepoMapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TDalEntity entity)
    {
        return RepoMapper.Map(RepositoryDbSet.Remove(RepoMapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Remove(TKey id)
    {
        var entity = FirstOrDefault(id);
        if (entity == null)
            throw new NullReferenceException($"Entity {typeof(TDalEntity).Name} with id {id} was not found");
        
        return Remove(entity);
    }

    public virtual TDalEntity? FirstOrDefault(TKey id)
    {
        return RepoMapper.Map(RepositoryDbSet.AsNoTracking().FirstOrDefault(entity => entity.Id.Equals(id)));
    }

    public virtual async Task<TDalEntity?> FirstOrDefaultAsync(TKey id)
    {
        return RepoMapper.Map(await RepositoryDbSet.AsNoTracking().FirstOrDefaultAsync(entity => entity.Id.Equals(id)));
    }

    public virtual async Task<IEnumerable<TDalEntity>> AllAsync()
    {
        return (await RepositoryDbSet.AsNoTracking().ToListAsync()).Select(entity => RepoMapper.Map(entity))!;
    }
}

public class BaseEntityRepository<TDalEntity, TDomainEntity, TDbContext> :
             BaseEntityRepository<TDalEntity, TDomainEntity, Guid, TDbContext>, IBaseRepository<TDalEntity>
    where TDalEntity : class, IDomainEntityId
    where TDomainEntity : class, IDomainEntityId<Guid>
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext, IMapper<TDalEntity, TDomainEntity> mapper) : base(dbContext, mapper)
    {
    }
}