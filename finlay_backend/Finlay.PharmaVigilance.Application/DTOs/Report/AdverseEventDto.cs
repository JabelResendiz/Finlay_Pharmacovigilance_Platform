

using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;


public class AdverseEventDto
{
    public required DateTime StartDate { get; set; }
    public required string Description { get; set; }
    public required SeverityLevel Severity { get; set; }
    public required bool RequiredHospitalization { get; set; }
    public required string Treatment { get; set; }
    public required string Notes { get; set; }
    public required string CurrentStatus { get; set; }
    public required List<SymptomDto> Symptoms { get; set; }
    
    // cuando se vaya a agregar evento adversos futuros, hay que asociarlo a un reporte
    //public required int AefiReportId {get;set;}
}