
using AutoMapper;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Domain.Entities;

namespace Finlay.PharmaVigilance.Application.DTO;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<RegisterUserDto, User>();

        CreateMap<User, UserResponseDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));


        CreateMap<User, GetUserDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.UserRole, opt => opt.MapFrom(src => src.UserRole));

        CreateMap<SymptomDto, Symptom>();

        CreateMap<PatientDto, Patient>();

        CreateMap<VaccineDto, Vaccine>();

        CreateMap<AdverseEventDto, AdverseEvent>();

        CreateMap<VaccinationDto, Vaccination>();

        CreateMap<ReportDto, AefiReport>();
    }
}

