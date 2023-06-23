using App.BLL.DTO;
using Base.Contracts.BLL;

namespace App.Contracts.BLL.Services;

public interface IOrderService : IEntityService<Order>
{
    public Task<IEnumerable<Order>> GetClientOrdersAsync(Guid clientId);
}