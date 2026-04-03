using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class AefiReport : GenericEntity
{
    public DateTime ReportDate { get; set; }

    public ReportStatus Status { get; set; }
    public string NotificationNumber { get; set; } = null!;
    public bool isMedicalReport { get; init; }


    public int ReporterId { get; set; }
    public Reporter Reporter { get; set; } = null!;

    public int VaccinatedSubjectId { get; set; }
    public VaccinatedSubject VaccinatedSubject { get; set; } = null!;

    public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();

    public MedicalReview? MedicalReview { get; set; }

    public ICollection<AdverseEvent> AdverseEvents { get; set; } = new List<AdverseEvent>();

    public int AlertId { get; set; }
    public Alert Alert { get; set; } = null!;

}