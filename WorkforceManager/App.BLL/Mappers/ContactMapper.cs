using Base.DAL;

namespace App.BLL.Mappers;

public class ContactMapper : BaseMapper<BLL.DTO.Contact, DAL.DTO.Contact>
{
    public ContactMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}