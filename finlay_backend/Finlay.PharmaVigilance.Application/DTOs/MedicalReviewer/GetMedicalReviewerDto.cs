using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class GetMedicalReviewerDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public int ProvinceId { get; set; }
    public int MunicipalityId { get; set; }
    public required string HealthArea { get; set; }
    public required string PhoneNumber { get; set; }

}