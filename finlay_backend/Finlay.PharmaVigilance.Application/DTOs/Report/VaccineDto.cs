using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class VaccineDto
{
    [Required(ErrorMessage = "Vaccine name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Vaccine name must be between 1 and 150 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Vaccine type is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Vaccine type must be between 1 and 50 characters.")]
    public string VaccineType { get; set; } = string.Empty;
    
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Manufacturer is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Manufacturer must be between 1 and 150 characters.")]
    public string Manufacturer { get; set; } = string.Empty;
}