using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class RegisterMedicalReviewerDto
{

    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "The email is not in a valid format.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gender is required")]
    public Gender Gender { get; set; }

    [Required(ErrorMessage = "Province is required")]
    public string Province { get; set; } = null!;

    [Required(ErrorMessage = "Municipality is required")]
    public string Municipality { get; set; } = null!;

    [Required(ErrorMessage = "HealthArea is required")]
    public string HealthArea { get; set; } = string.Empty;

    [Required(ErrorMessage = "PhoneNumber is required")]
    public string PhoneNumber { get; set; } = string.Empty;

}