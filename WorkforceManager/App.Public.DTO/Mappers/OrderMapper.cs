using Base.DAL;

namespace App.Public.DTO.Mappers;

public class OrderMapper : BaseMapper<Public.DTO.v1.Order, BLL.DTO.Order>
{
    public OrderMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
    
    public Public.DTO.v1.Order MapWithClient(BLL.DTO.Order order)
    {
        return Mapper.Map<Public.DTO.v1.Order>(new Public.DTO.v1.Order
        {
            Id = order.Id,
            ClientId = order.Client!.Id,
            Name = order.Name,
            ClientName = order.Client!.Name,
            TotalTasks = order.TotalTasks,
            Start = order.Start,
            End = order.End
        });
    }
}