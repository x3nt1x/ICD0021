using Base.DAL;

namespace App.DAL.Mappers;

public class WorkerMapper : BaseMapper<DAL.DTO.Worker, Domain.Worker>
{
    public WorkerMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}