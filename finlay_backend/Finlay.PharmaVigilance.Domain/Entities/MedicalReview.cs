using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class MedicalReview : GenericEntity
{
    public int MedicalReviewAssignmentId { get; set; }
    public ClinicalSignificance ClinicalSignificance { get; set; }
    public CausalityLevel Causality { get; set; }
    public DateTime ReviewedAt { get; set; }

    public MedicalReviewAssignment MedicalReviewAssignment { get; set; } = null!;
}