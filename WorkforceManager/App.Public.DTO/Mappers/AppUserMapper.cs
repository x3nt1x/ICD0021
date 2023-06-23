using Base.DAL;

namespace App.Public.DTO.Mappers;

public class AppUserMapper : BaseMapper<Public.DTO.v1.Identity.AppUser, Domain.Identity.AppUser>
{
    public AppUserMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}