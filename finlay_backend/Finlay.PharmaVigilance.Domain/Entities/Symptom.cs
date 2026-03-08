

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Symptom : GenericEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string StandardCode { get; set; } = null!;
    //public DateTime CreatedAt { get; set; }

    // public int AdverseEvent_SymptomId { get; set; }
    public ICollection<AdverseEventSymptom> AdverseEventSymptoms { get; set; } = new List<AdverseEventSymptom>();

    //public ICollection<AdverseEvent> AdverseEvents { get; set; } = new List<AdverseEvent>();
}