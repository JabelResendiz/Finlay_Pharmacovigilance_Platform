namespace Finlay.PharmaVigilance.Domain.Entities;

public class MedicalReview : GenericEntity
{
    public int AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;

    public int MedicalReviewerId { get; set; }
    public MedicalReviewer MedicalReviewer { get; set; } = null!;


    public string ClinicalDescription { get; set; } = null!;
    public float? Temperature { get; set; }
    public string MedDraTerm { get; set; } = null!;

    public DateTime ReviewedAt { get; set; }

}