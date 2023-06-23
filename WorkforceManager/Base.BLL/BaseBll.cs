using Base.Contracts.BLL;
using Base.Contracts.DAL;

namespace Base.BLL;

public abstract class BaseBll<TUnitOfWork> : IBaseBll
    where TUnitOfWork : IBaseUow
{
    protected readonly TUnitOfWork Uow;

    protected BaseBll(TUnitOfWork uow)
    {
        Uow = uow;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await Uow.SaveChangesAsync();
    }
}