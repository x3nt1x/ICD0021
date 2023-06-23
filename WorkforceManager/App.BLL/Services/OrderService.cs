using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using Base.BLL;
using Base.Contracts;

namespace App.BLL.Services;

public class OrderService : BaseEntityService<BLL.DTO.Order, DAL.DTO.Order, IOrderRepository>, IOrderService
{
    public OrderService(IOrderRepository repository, IMapper<BLL.DTO.Order, DAL.DTO.Order> mapper) : base(repository, mapper)
    {
    }

    public override async Task<IEnumerable<Order>> AllAsync()
    {
        return (await Repository.AllAsync()).Select(order => Mapper.Map(order)).ToList()!;
    }

    public async Task<IEnumerable<Order>> GetClientOrdersAsync(Guid clientId)
    {
        return (await Repository.GetClientOrdersAsync(clientId))
            .Select(order => Mapper.Map(order))
            .ToList()!;
    }
}