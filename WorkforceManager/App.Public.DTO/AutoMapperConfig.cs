using AutoMapper;

namespace App.Public.DTO;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Public.DTO.v1.Assignment, BLL.DTO.Assignment>().ReverseMap();
        CreateMap<Public.DTO.v1.Client, BLL.DTO.Client>().ReverseMap();
        CreateMap<Public.DTO.v1.Comment, BLL.DTO.Comment>().ReverseMap();
        CreateMap<Public.DTO.v1.Contact, BLL.DTO.Contact>().ReverseMap();
        CreateMap<Public.DTO.v1.Order, BLL.DTO.Order>().ReverseMap();
        CreateMap<Public.DTO.v1.Worker, BLL.DTO.Worker>().ReverseMap();
        CreateMap<Public.DTO.v1.Identity.AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
}