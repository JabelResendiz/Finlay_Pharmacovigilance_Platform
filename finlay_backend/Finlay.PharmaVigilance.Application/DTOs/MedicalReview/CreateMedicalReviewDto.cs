using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;


public class CreateMedicalReviewDto
{
    [Required(ErrorMessage = "Aefi Report Id is required")]
    public int AefiReportId { get; set; }

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