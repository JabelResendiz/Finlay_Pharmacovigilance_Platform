
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

        // Medical Reviewer Registration
        CreateMap<RegisterMedicalReviewerDto, MedicalReviewer>();

        // Section Responsible Registration
        CreateMap<RegisterSectionResponsibleDto, SectionResponsible>();


        // Report Mappings
        
        CreateMap<VaccineDto, Vaccine>();
        CreateMap<VaccinationDto, Vaccination>();

        CreateMap<VaccinatedSubjectDto,VaccinatedSubject>();
        CreateMap<ReporterDto, Reporter>();

        CreateMap<AdverseEventDto, AdverseEvent>()
             .ForMember(dest => dest.AdverseEventSymptoms, 
                        opt => opt.MapFrom(
                            src => src.Symptoms.Select(
                                s => new AdverseEventSymptom { 
                                    Symptom = new Symptom { 
                                        Name = s.Name, 
                                        Description = s.Description, 
                                        StandardCode = s.StandardCode 
                                    } 
                                }
                            )
                        )
                    );
        CreateMap<SymptomDto, Symptom>();


        CreateMap<ReportDto, AefiReport>();


    }
}