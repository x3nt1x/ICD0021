using Base.DAL;

namespace App.DAL.Mappers;

public class OrderMapper : BaseMapper<DAL.DTO.Order, Domain.Order>
{
    public OrderMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}