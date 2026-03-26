using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReporterDto
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(150, MinimumLength = 1, ErrorMessage = "Full name must be between 1 and 150 characters.")]

    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Reporter Relationship is required.")]
    public ReporterRelationship ReporterRelationship { get; set; }

    [Required(ErrorMessage = "Date Of Birth is required.")]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Province Id is required.")]
    public int ProvinceId { get; set; }

    [Required(ErrorMessage = "Municipality Id is required.")]
    public int MunicipalityId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}