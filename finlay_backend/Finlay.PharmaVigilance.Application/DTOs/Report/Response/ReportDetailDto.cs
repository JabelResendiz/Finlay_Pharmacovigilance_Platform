using Finlay.PharmaVigilance.Domain.Entities;
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Application.DTO;

public class ReportUserDto
{
    public required DateTime ReportDate { get; set; }
    public required DateTime CreatedAt { get; set; }
    public DateTime? AssignedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public required ReportStatus Status { get; set; }
    public required VaccinatedSubjectSummaryDto VaccinatedSubject { get; set; }
    public required ReporterDetailsDto Reporter { get; set; }
    public required IEnumerable<VaccinationDetailsDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventDetailDto> AdverseEvents { get; set; }
}


public class ReportMedicalReviewerDto
{

    public required Guid Id { get; set; }

    public required DateTime ReportDate { get; set; }
    public required ReportStatus Status { get; set; }
    public required VaccinatedSubjectSummaryDto VaccinatedSubject { get; set; }
    public required ReporterDetailsDto Reporter { get; set; }
    public required IEnumerable<VaccinationDetailsDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventDetailMedicalReviewerDto> AdverseEvents { get; set; }
}


public class ReportAdminDto
{

    public required Guid Id { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ReportDate { get; set; }
    public required ReportStatus Status { get; set; }
    public required ReporterAdminDto Reporter { get; set; }
    public required VaccinatedSubjectAdminDto VaccinatedSubject { get; set; }
    public required IEnumerable<VaccinationDetailsDto> Vaccinations { get; set; }
    public required IEnumerable<AdverseEventAdminDto> AdverseEvents { get; set; }
    public required IEnumerable<AssignmentResponse> MedicalReviewAssignments { get; set; }
    public MedicalReviewResponseDto? MedicalReview { get; set; }

}
