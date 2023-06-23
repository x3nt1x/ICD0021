using Base.DAL;

namespace App.Public.DTO.Mappers;

public class CommentMapper : BaseMapper<Public.DTO.v1.Comment, BLL.DTO.Comment>
{
    public CommentMapper(AutoMapper.IMapper mapper) : base(mapper)
    {
    }
    
    public Public.DTO.v1.Comment MapWithAppUser(BLL.DTO.Comment comment)
    {
        return Mapper.Map<Public.DTO.v1.Comment>(new Public.DTO.v1.Comment
        {
            Id = comment.Id,
            Content = comment.Content,
            Date = comment.Date,
            FirstName = comment.AppUser!.FirstName,
            LastName = comment.AppUser!.LastName
        });
    }
}