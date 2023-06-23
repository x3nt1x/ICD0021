using Base.DAL;

namespace App.BLL.Mappers;

public class AssignmentMapper : BaseMapper<BLL.DTO.Assignment, DAL.DTO.Assignment>
{
    public AssignmentMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}