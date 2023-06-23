using Base.DAL;

namespace App.Public.DTO.Mappers;

public class ContactMapper : BaseMapper<Public.DTO.v1.Contact, BLL.DTO.Contact>
{
    public ContactMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}