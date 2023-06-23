using Base.DAL;

namespace App.BLL.Mappers;

public class OrderMapper : BaseMapper<BLL.DTO.Order, DAL.DTO.Order>
{
    public OrderMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}