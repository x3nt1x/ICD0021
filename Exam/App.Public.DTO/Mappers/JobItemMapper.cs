using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class JobItemMapper : BaseMapper<Public.DTO.JobItem, Domain.JobItem>
{
    public JobItemMapper(IMapper mapper) : base(mapper)
    {
    }
}