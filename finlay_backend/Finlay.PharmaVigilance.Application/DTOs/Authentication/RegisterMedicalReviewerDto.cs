using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

/// <summary>
/// DTO for registering a new Medical Reviewer user with their specific profile information.
/// </summary>
public class RegisterMedicalReviewerDto : RegisterUserDto
{
    /// <summary>
    /// Health area where the Medical Reviewer works.
    /// </summary>
    [Required(ErrorMessage = "Institution is required")]
    public string Institution { get; set; } = string.Empty;

    [Required(ErrorMessage = "Professional License is required")]
    public string ProfessionalLicense { get; set; } = string.Empty;

    public string? Specialty { get; set; }

}
