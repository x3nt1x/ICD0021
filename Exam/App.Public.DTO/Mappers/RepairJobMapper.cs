using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class RepairJobMapper : BaseMapper<Public.DTO.RepairJob, Domain.RepairJob>
{
    public RepairJobMapper(IMapper mapper) : base(mapper)
    {
    }
}