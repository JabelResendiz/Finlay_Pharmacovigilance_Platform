using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class VaccinatedSubject : GenericEntity
{
    public string FullName { get; set; } = null!;
    public string IdentityNumber { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }

    public Gender Gender { get; set; }
    public bool? IsPregnant { get; set; }

    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;

    public int MunicipalityId { get; set; }
    public Municipality Municipality { get; set; } = null!;

    public string? HealthArea { get; set; }

    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }

}