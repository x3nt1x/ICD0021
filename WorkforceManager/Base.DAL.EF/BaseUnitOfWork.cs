using Base.Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseUnitOfWork<TDbContext> : IBaseUow
    where TDbContext : DbContext
{
    protected readonly TDbContext DbContext;

    public BaseUnitOfWork(TDbContext dbContext)
    {
        DbContext = dbContext;
    }
    
    public virtual async Task<int> SaveChangesAsync()
    {
        return await DbContext.SaveChangesAsync();
    }
}