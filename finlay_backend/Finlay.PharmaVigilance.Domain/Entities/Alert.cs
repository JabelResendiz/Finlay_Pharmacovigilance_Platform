namespace Finlay.PharmaVigilance.Domain.Entities;

public class Alert : GenericEntity
{
    public string Description { get; set; } = null!;
    public DateTime? ReadAt { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsRead { get; set; } = false;


    //FK
    public int SectionResponsibleId { get; set; }
    public SectionResponsible SectionResponsible { get; set; } = null!;
    public int AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;
}