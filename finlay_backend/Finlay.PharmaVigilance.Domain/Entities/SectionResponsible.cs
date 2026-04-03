namespace Finlay.PharmaVigilance.Domain.Entities;

public class SectionResponsible : GenericEntity
{
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
    public int MunicipalityId { get; set; }
    public Municipality Municipality { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public ICollection<Alert> ReceivedAlerts { get; set; } = new List<Alert>();
    public ICollection<MedicalReview> ManagedReviews { get; set; } = new List<MedicalReview>();
    public ICollection<MedicalReviewer> MedicalReviewers { get; set; } = new List<MedicalReviewer>();
    public int AdminId { get; set; }
    public Admin Admin { get; set; } = null!;

}