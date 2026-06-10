public class AefiReport : GuidEntity
{
    public DateTime ReportDate { get; set; }
    public ReportStatus Status { get; set; }
    public string NotificationNumber { get; set; } = null!;
    public bool isMedicalReport { get; set; }
    public string IdempotencyKey { get; set; } = null!;

    public Guid ReporterId { get; set; }
    public Guid VaccinatedSubjectId { get; set; }

    public Reporter Reporter { get; set; } = null!;

    public VaccinatedSubject VaccinatedSubject { get; set; } = null!;


    public ICollection<Vaccination> Vaccinations { get; set; } = new List<Vaccination>();
    public ICollection<AdverseEvent> AdverseEvents { get; set; } = new List<AdverseEvent>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
    public ICollection<MedicalReviewAssignment> MedicalReviewAssignments { get; set; } = new List<MedicalReviewAssignment>();


    public ReportPriority Priority =>
        AdverseEvents.Any()
            ? AdverseEvents.Max(ae => ae.GetPriority())
            : ReportPriority.Low;
}


public class AdverseEvent : GuidEntity
{
    public DateTime StartDate { get; set; }
    public DateTime? FinishDate { get; set; }

    public string? Description { get; set; }

    public bool VisitedDoctor { get; set; } = false;
    public bool WentToEmergencyRoom { get; set; } = false;
    public bool PermanentDisability { get; set; } = false;
    public bool Anomaly { get; set; } = false;
    public bool WasHospitalized { get; set; } = false;
    public bool ResultedInDeath { get; set; } = false;
    public bool NoComplications { get; set; } = true;
    public DateTime? DeathDate { get; set; }
    public PatientStatus CurrentStatus { get; set; }
    public Intensity Intensity { get; set; }
    public SeverityLevel SeverityLevel { get; set; }

    public string? LaboratoryResults { get; set; }
    public string? MedDRACode { get; set; } = null!;
    public string? RetClassification { get; set; } = null!;

    public Guid AefiReportId { get; set; }
    public AefiReport AefiReport { get; set; } = null!;

    public Guid SymptomId { get; set; }
    public Symptom Symptom { get; set; } = null!;

    public ReportPriority GetPriority()
    {
        // Prioridad alta: evento serio o criterios de peligro
        if (SeverityLevel == SeverityLevel.Serious || CurrentStatus == PatientStatus.Fatal)
            return ReportPriority.High;

        // Prioridad media: no serio pero intensidad severa o paciente no recuperado
        if (Intensity == Intensity.Severe ||
            CurrentStatus == PatientStatus.NotRecovered ||
            CurrentStatus == PatientStatus.RecoveredWithSequelae)
            return ReportPriority.Medium;

        // Prioridad baja: resto de casos
        return ReportPriority.Low;
    }
}


public class MedicalReviewAssignment : GuidEntity
{
    public Guid SectionResponsibleId { get; set; }
    public Guid MedicalReviewerId { get; set; }
    public Guid AefiReportId { get; set; }
    public DateTime AssignedAt { get; set; }
    public ReviewAssignmentStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public byte[] RowVersion { get; set; } = null!;

    public SectionResponsible SectionResponsible { get; set; } = null!;
    public MedicalReviewer MedicalReviewer { get; set; } = null!;
    public AefiReport AefiReport { get; set; } = null!;


    public MedicalReview? MedicalReview { get; set; }

}


public class MedicalReview : GuidEntity
{
    public Guid MedicalReviewAssignmentId { get; set; }
    public ClinicalSignificance ClinicalSignificance { get; set; }
    public CausalityLevel Causality { get; set; }
    public DateTime ReviewedAt { get; set; }

    public MedicalReviewAssignment MedicalReviewAssignment { get; set; } = null!;
}