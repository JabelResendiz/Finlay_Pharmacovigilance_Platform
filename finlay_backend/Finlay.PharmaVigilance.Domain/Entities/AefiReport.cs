using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class AefiReport : GenericEntity
{
    public DateTime ReportDate { get; set; }

    public int ReporterId { get; set; }
    //public int MedicalReviewerId { get; set; }
    public int VaccinatedSubjectId { get; set; }
    public int VaccinationId { get; set; }

    public Reporter Reporter { get; set; } = null!;
    //public MedicalReviewer MedicalReviewer { get; set; } = null!;
    public VaccinatedSubject VaccinatedSubject { get; set; } = null!;

    public ReportStatus Status { get; set; }
    public string NotificationNumber { get; set; } = null!;

    public Vaccination Vaccination { get; set; } = null!;
    public ICollection<AdverseEvent> AdverseEvents { get; set; } = new List<AdverseEvent>();


    public MedicalReview? MedicalReview { get; set; }
}