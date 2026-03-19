
namespace Finlay.PharmaVigilance.Application.DTO;


public class ReportDto
{
    public required DateTime ReportDate { get; set; }
    public required ReporterDto Reporter { get; set; }
    public required VaccinatedSubjectDto VaccinatedSubject { get; set; }
    public required VaccinationDto Vaccination { get; set; }
    public required List<AdverseEventDto> AdverseEvents { get; set; }

    //public required MedicalReviewerDto MedicalReviewerDto {get;set;}
    
}