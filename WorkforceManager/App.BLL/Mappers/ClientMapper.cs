using Base.DAL;

namespace App.BLL.Mappers;

public class ClientMapper : BaseMapper<App.BLL.DTO.Client, App.DAL.DTO.Client>
{
    public ClientMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}