namespace Finlay.PharmaVigilance.Domain.Entities;


public class AefiReport : GenericEntity
{
    public DateTime ReportDate { get; set; }
    public string GeneralNotes { get; set; } = null!;
    // public DateTime CreatedAt { get; set; }



    public int PhysicianId { get; set; }
    public int PatientId { get; set; }
    public int VaccinationId { get; set; }


    public Physician Physician { get; set; } = null!;
    public Patient Patient { get; set; } = null!;
    public Vaccination Vaccination { get; set; } = null!;
    public ICollection<AdverseEvent> AdverseEvents { get; set; } = new List<AdverseEvent>();

}