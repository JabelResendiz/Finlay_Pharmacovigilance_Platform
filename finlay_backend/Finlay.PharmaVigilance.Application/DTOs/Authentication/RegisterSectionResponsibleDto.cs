using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

/// <summary>
/// DTO for registering a new Section Responsible user with their specific profile information.
/// </summary>
public class RegisterSectionResponsibleDto : RegisterUserDto
{
    /// <summary>
    /// Province identifier for the Section Responsible.
    /// </summary>
    [Required(ErrorMessage = "Province ID is required")]
    public int ProvinceId { get; set; }

    [Required(ErrorMessage = "Municipality ID is required")]
    public int MunicipalityId { get; set; }

}
