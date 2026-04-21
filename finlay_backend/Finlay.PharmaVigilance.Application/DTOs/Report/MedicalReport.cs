using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class MedicalReportDto : ReportDto
{

    // [Required(ErrorMessage = "Report date is required.")]
    // public DateTime? ReportDate { get; set; }

    // [Required(ErrorMessage = "Vaccinated subject information is required.")]
    // public VaccinatedSubjectDto VaccinatedSubject { get; set; } = null!;

    // [Required(ErrorMessage = "At least one adverse event is required.")]
    // [MinLength(1, ErrorMessage = "At least one adverse event must be provided.")]
    // public List<VaccinationDto> Vaccinations { get; set; } = new();

    // [Required(ErrorMessage = "At least one adverse event is required.")]
    // [MinLength(1, ErrorMessage = "At least one adverse event must be provided.")]
    // public List<AdverseEventDto> AdverseEvents { get; set; } = new();

    [Required(ErrorMessage = "Causality is required.")]
    public CausalityLevel? Causality { get; set; }
    [Required(ErrorMessage = "Clinical Significance is required.")]
    public ClinicalSignificance? ClinicalSignificance { get; set; }
}