namespace Finlay.PharmaVigilance.Domain.Events;

public class ReportCreatedEvent
{
    public string ReportNumber { get; set; } = null!;
    public string ReporterEmail { get; set; } = null!;
    public string SectionResponsibleEmail { get; set; } = null!;

}

