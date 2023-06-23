using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class ItemMapper : BaseMapper<Public.DTO.Item, Domain.Item>
{
    public ItemMapper(IMapper mapper) : base(mapper)
    {
    }
}