using Base.DAL;

namespace App.Public.DTO.Mappers;

public class AssignmentMapper : BaseMapper<Public.DTO.v1.Assignment, BLL.DTO.Assignment>
{
    public AssignmentMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}