
namespace Finlay.PharmaVigilance.Domain.Entities;

public class AdverseEventSymptom : GenericEntity
{
    public int AdverseEventId { get; set; }
    public AdverseEvent AdverseEvent { get; set; } = null!;
    public int SymptomId { get; set; }
    public Symptom Symptom { get; set; } = null!;

    public string? SpecificDetail { get; set; }
}