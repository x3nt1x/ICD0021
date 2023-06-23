using Base.DAL;

namespace App.BLL.Mappers;

public class WorkerMapper : BaseMapper<BLL.DTO.Worker, DAL.DTO.Worker>
{
    public WorkerMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}