using Base.DAL;

namespace App.DAL.Mappers;

public class ClientMapper : BaseMapper<DAL.DTO.Client, Domain.Client>
{
    public ClientMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}