using Base.DAL;

namespace App.Public.DTO.Mappers;

public class WorkerMapper : BaseMapper<Public.DTO.v1.Worker, BLL.DTO.Worker>
{
    public WorkerMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
    
    public Public.DTO.v1.Worker MapWithAppUser(BLL.DTO.Worker worker)
    {
        return Mapper.Map<Public.DTO.v1.Worker>(new Public.DTO.v1.Worker
        {
            Id = worker.Id,
            AppUserId = worker.AppUser!.Id,
            FirstName = worker.AppUser!.FirstName,
            LastName = worker.AppUser!.LastName
        });
    }
}