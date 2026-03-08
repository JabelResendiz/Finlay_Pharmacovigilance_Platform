using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class AdverseEvent : GenericEntity
{
    public DateTime StartDate { get; set; }
    public string Description { get; set; } = null!;
    public SeverityLevel Severity { get; set; }
    public bool RequiredHospitalization { get; set; }
    public string Treatment { get; set; } = null!;
    public string Notes { get; set; } = null!;
    public string CurrentStatus { get; set; } = null!;
    // public DateTime CreatedAt { get; set; }

    public int AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;
    //public int AdverseEvent_SymptomId { get; set; }

    public ICollection<AdverseEventSymptom> AdverseEventSymptoms { get; set; } = new List<AdverseEventSymptom>();
    //public ICollection<Symptom> Symptoms { get; set; } = new List<Symptom>();
}