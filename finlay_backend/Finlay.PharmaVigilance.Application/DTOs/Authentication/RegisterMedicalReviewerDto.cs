using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

/// <summary>
/// DTO for registering a new Medical Reviewer user with their specific profile information.
/// </summary>
public class RegisterMedicalReviewerDto : RegisterUserDto
{
    /// <summary>
    /// Date of birth of the Medical Reviewer.
    /// </summary>
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gender of the Medical Reviewer.
    /// </summary>
    [Required(ErrorMessage = "Gender is required")]
    public Gender Gender { get; set; }

    /// <summary>
    /// Health area where the Medical Reviewer works.
    /// </summary>
    [Required(ErrorMessage = "Health area is required")]
    public string HealthArea { get; set; } = string.Empty;

    /// <summary>
    /// Phone number of the Medical Reviewer.
    /// </summary>
    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Phone number is not in a valid format.")]
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Province identifier where the Medical Reviewer operates.
    /// </summary>
    [Required(ErrorMessage = "Province ID is required")]
    public int ProvinceId { get; set; }

    /// <summary>
    /// Municipality identifier where the Medical Reviewer operates.
    /// </summary>
    [Required(ErrorMessage = "Municipality ID is required")]
    public int MunicipalityId { get; set; }
}
