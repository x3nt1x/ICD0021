using Base.DAL;

namespace App.DAL.Mappers;

public class AssignmentMapper : BaseMapper<DAL.DTO.Assignment, Domain.Assignment>
{
    public AssignmentMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}