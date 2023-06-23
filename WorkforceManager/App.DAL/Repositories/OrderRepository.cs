using App.Contracts.DAL;
using App.DAL.DTO;
using Base.Contracts;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Repositories;

public class OrderRepository : BaseEntityRepository<DAL.DTO.Order, Domain.Order, AppDbContext>, IOrderRepository
{
    public OrderRepository(AppDbContext dbContext, IMapper<DAL.DTO.Order, Domain.Order> mapper) : base(dbContext, mapper)
    {
    }

    public async Task<Order?> FindAsync(Guid id)
    {
        return RepoMapper.Map(await RepositoryDbSet.Where(order => order.Id == id).FirstOrDefaultAsync());
    }

    public override async Task<IEnumerable<Order>> AllAsync()
    {
        return (await RepositoryDbSet
            .AsNoTracking()
            .Include(order => order.Client)
            .Select(order => RepoMapper.Map(order))
            .ToListAsync())!;
    }

    public async Task<IEnumerable<Order>> GetClientOrdersAsync(Guid clientId)
    {
        return (await RepositoryDbSet
            .AsNoTracking()
            .Where(order => order.ClientId.Equals(clientId))
            .Select(order => RepoMapper.Map(order))
            .ToListAsync())!;
    }
}