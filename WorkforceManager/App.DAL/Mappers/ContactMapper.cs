using Base.DAL;

namespace App.DAL.Mappers;

public class ContactMapper : BaseMapper<DAL.DTO.Contact, Domain.Contact>
{
    public ContactMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}