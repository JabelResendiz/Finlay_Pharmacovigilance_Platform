
namespace Finlay.PharmaVigilance.Domain.Entities;


public class MedicalReviewer : GenericEntity
{
   public int ProvinceId { get; set; }
   public Province Province { get; set; } = null!;
   public int MunicipalityId { get; set; }
   public Municipality Municipality { get; set; } = null!;
   public string Institution { get; set; } = null!;

   // FK
   public int UserId { get; set; }
   public User User { get; set; } = null!;
   public string ProfessionalLicense { get; set; } = null!;
   public string? Specialty { get; set; }

   public int SectionResponsibleId { get; set; }
   public SectionResponsible SectionResponsible { get; set; } = null!;

   public ICollection<MedicalReview> MedicalReviews { get; set; } = new List<MedicalReview>();

}