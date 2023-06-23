using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class RepairMapper : BaseMapper<Public.DTO.Repair, Domain.Repair>
{
    public RepairMapper(IMapper mapper) : base(mapper)
    {
    }
}