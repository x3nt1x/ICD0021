using AutoMapper;
using Base.DAL;

namespace App.Public.DTO.Mappers;

public class JobMapper : BaseMapper<Public.DTO.Job, Domain.Job>
{
    public JobMapper(IMapper mapper) : base(mapper)
    {
    }
}