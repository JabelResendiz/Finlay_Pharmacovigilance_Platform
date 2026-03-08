

namespace Finlay.PharmaVigilance.Application.DTO;


public class VaccinationDto
{

    public required VaccineDto Vaccine
    {
        get; set;
    }

    public required string BatchNumber { get; set; }
    public required string AdministrationSite { get; set; }
    public required int DoseNumber { get; set; }
    public required DateTime AdministrationDate { get; set; }

}