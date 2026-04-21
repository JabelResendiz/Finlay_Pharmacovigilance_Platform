using System.ComponentModel.DataAnnotations;

namespace Finlay.PharmaVigilance.Application.DTO;

public class MedicalReviewAssignmentDTO
{
    [Required(ErrorMessage = "Medical Reviewer Id is required")]
    public int MedicalReviewerId { get; set; }

    [Required(ErrorMessage = "Aefi Report Id is required")]
    public int AefiReportId { get; set; }

    [Required(ErrorMessage = "Assigned At is required")]
    public DateTime? AssignedAt { get; set; }

}