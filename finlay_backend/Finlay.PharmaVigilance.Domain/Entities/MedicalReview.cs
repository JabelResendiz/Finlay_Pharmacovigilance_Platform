using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class MedicalReview : GenericEntity
{
    public int AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;

    public int MedicalReviewerId { get; set; }
    public MedicalReviewer MedicalReviewer { get; set; } = null!;

    public ClinicalSignificance ClinicalSignificance { get; set; }
    public CausalityLevel Causality { get; set; }

    public DateTime ReviewedAt { get; set; }
    public int SectionResponsibleId { get; set; }
    public SectionResponsible SectionResponsible { get; set; } = null!;

}