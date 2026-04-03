using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class Vaccination : GenericEntity
{
    public string BatchNumber { get; set; } = null!;
    public AdministrationSite Site { get; set; }
    public int DoseNumber { get; set; }
    public DateTime AdministrationDate { get; set; }
    public string? VaccinationCenter { get; set; }

    public int VaccineId { get; set; }
    public Vaccine Vaccine { get; set; } = null!;
    public int AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;

}