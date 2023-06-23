using Base.DAL;

namespace App.BLL.Mappers;

public class CommentMapper : BaseMapper<BLL.DTO.Comment, DAL.DTO.Comment>
{
    public CommentMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}