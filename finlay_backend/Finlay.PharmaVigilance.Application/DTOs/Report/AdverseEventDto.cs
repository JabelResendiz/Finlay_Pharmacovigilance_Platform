

using System.ComponentModel.DataAnnotations;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class AdverseEventDto
{

    [Required(ErrorMessage = "Start date is required.")]
    public DateTime StartDate { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Severity level is required.")]
    public SeverityLevel Severity { get; set; }

    [Required]
    public bool RequiredHospitalization { get; set; }

    [Required]
    [StringLength(500, MinimumLength = 1)]
    public string Treatment { get; set; } = string.Empty;

    [Required]
    [StringLength(1000, MinimumLength = 1)]
    public string Notes { get; set; } = string.Empty;

    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string CurrentStatus { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "At least one symptom is required.")]
    public List<SymptomDto> Symptoms { get; set; } = new();

    // cuando se vaya a agregar evento adversos futuros, hay que asociarlo a un reporte
    //public required int AefiReportId {get;set;}
}