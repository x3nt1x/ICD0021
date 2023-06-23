using Base.DAL;

namespace App.Public.DTO.Mappers;

public class ClientMapper : BaseMapper<Public.DTO.v1.Client, BLL.DTO.Client>
{
    public ClientMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}