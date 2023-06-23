using Base.DAL;

namespace App.Public.DTO.Mappers;

public class AppUserMapper : BaseMapper<Public.DTO.Identity.AppUser, Domain.Identity.AppUser>
{
    public AppUserMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}