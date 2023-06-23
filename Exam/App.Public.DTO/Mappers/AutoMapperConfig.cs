using AutoMapper;

namespace App.Public.DTO.Mappers;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Public.DTO.Item, Domain.Item>().ReverseMap();
        CreateMap<Public.DTO.Job, Domain.Job>().ReverseMap();
        CreateMap<Public.DTO.JobItem, Domain.JobItem>().ReverseMap();
        CreateMap<Public.DTO.Repair, Domain.Repair>().ReverseMap();
        CreateMap<Public.DTO.RepairJob, Domain.RepairJob>().ReverseMap();
        CreateMap<Public.DTO.Identity.AppUser, Domain.Identity.AppUser>().ReverseMap();
    }
}