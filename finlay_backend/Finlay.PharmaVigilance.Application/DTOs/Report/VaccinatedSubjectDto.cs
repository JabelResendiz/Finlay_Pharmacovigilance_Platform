

using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class VaccinatedSubjectDto
{
    public required string FullName { get; set; }
    public required string IdentityNumber {get;set;}
    public required DateTime DateOfBirth{get; set;}


    public required Gender Gender { get; set; }
    public required bool? IsPregnant { get; set; }

    public required int ProvinceId { get; set; }
    public required int MunicipalityId { get; set; }


    public required string? HealthArea { get; set; }
    public required string? Address { get; set; }
    public required string? PhoneNumber { get; set; }
    public required string? Email { get; set; }


}