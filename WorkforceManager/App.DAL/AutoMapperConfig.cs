using AutoMapper;

namespace App.DAL;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<DAL.DTO.Assignment, Domain.Assignment>().ReverseMap();
        CreateMap<DAL.DTO.Client, Domain.Client>().ReverseMap();
        CreateMap<DAL.DTO.Comment, Domain.Comment>().ReverseMap();
        CreateMap<DAL.DTO.Contact, Domain.Contact>().ReverseMap();
        CreateMap<DAL.DTO.Order, Domain.Order>().ReverseMap();
        CreateMap<DAL.DTO.Worker, Domain.Worker>().ReverseMap();
    }
}