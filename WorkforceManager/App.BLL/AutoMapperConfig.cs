using AutoMapper;

namespace App.BLL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<BLL.DTO.Assignment, DAL.DTO.Assignment>().ReverseMap();
        CreateMap<BLL.DTO.Client, DAL.DTO.Client>().ReverseMap();
        CreateMap<BLL.DTO.Comment, DAL.DTO.Comment>().ReverseMap();
        CreateMap<BLL.DTO.Contact, DAL.DTO.Contact>().ReverseMap();
        CreateMap<BLL.DTO.Order, DAL.DTO.Order>().ReverseMap();
        CreateMap<BLL.DTO.Worker, DAL.DTO.Worker>().ReverseMap();
    }
}