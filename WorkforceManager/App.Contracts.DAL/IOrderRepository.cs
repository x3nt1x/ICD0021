using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL;

public interface IOrderRepository : IBaseRepository<Order>
{
    public Task<Order?> FindAsync(Guid id);
    public Task<IEnumerable<Order>> GetClientOrdersAsync(Guid clientId);
}