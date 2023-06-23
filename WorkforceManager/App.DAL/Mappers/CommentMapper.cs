using Base.DAL;

namespace App.DAL.Mappers;

public class CommentMapper : BaseMapper<DAL.DTO.Comment, Domain.Comment>
{
    public CommentMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
}