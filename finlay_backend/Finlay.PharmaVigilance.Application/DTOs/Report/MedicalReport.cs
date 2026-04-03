using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class MedicalReportDto
{

    [Required(ErrorMessage = "Report date is required.")]
    public DateTime ReportDate { get; set; }

    [Required(ErrorMessage = "Vaccinated subject information is required.")]
    public VaccinatedSubjectDto VaccinatedSubject { get; set; } = null!;

    [Required(ErrorMessage = "Vaccination information is required.")]
    public VaccinationDto Vaccination { get; set; } = null!;

    [Required(ErrorMessage = "At least one adverse event is required.")]
    [MinLength(1, ErrorMessage = "At least one adverse event must be provided.")]
    public List<AdverseEventDto> AdverseEvents { get; set; } = new();

    [Required(ErrorMessage = "Clinical Description is required.")]
    [StringLength(1000, MinimumLength = 1, ErrorMessage = "Clinical Description must be between 1 and 1000 characters.")]
    public string ClinicalDescription { get; set; } = string.Empty;

    [Required(ErrorMessage = "MedDraTerm is required.")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "MedDraTerm must be between 1 and 200 characters.")]
    public string MedDraTerm { get; set; } = string.Empty;
    public float? Temperature { get; set; }

    [Required(ErrorMessage = "Reviewed At is required.")]
    public DateTime ReviewedAt { get; set; }
}