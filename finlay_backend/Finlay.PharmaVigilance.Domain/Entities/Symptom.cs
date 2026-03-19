

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Symptom : GenericEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string StandardCode { get; set; } = null!;

    public ICollection<AdverseEventSymptom> AdverseEventSymptoms { get; set; } = new List<AdverseEventSymptom>();

}