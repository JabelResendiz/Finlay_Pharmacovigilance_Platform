using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class AdverseEvent : GenericEntity
{
    public DateTime StartDate { get; set; }

    public string Description { get; set; } = null!;

    public bool VisitedDoctor { get; set; } = false;
    public bool WentToEmergencyRoom { get; set; } = false;
    public bool PermanentDisability { get; set; } = false;
    public bool IsLifeThreatening { get; set; } = false;
    public bool ResultedInDeath { get; set; } = false;
    public DateTime? DeathDate { get; set; }
    public PatientStatus CurrentStatus { get; set; }


    public string? LaboratoryResults { get; set; }

    public string? MedDRACode { get; set; } = null!;
    public string? RetClassification { get; set; } = null!;



    public int AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;
    public ICollection<AdverseEventSymptom> AdverseEventSymptoms { get; set; } = new List<AdverseEventSymptom>();

}