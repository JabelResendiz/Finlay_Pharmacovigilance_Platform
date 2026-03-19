using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccinationDto
{
    [Required(ErrorMessage = "Vaccine information is required.")]
    public VaccineDto Vaccine { get; set; } = null!;

    [Required(ErrorMessage = "Batch number is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Batch number must be between 1 and 50 characters.")]
    public string BatchNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Administration site is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Administration site must be between 1 and 100 characters.")]
    public string AdministrationSite { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Dose number must be greater than 0.")]
    public int DoseNumber { get; set; }

    [Required(ErrorMessage = "Administration date is required.")]
    public DateTime AdministrationDate { get; set; }
}