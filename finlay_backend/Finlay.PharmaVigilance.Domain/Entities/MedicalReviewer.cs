
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class MedicalReviewer : GenericEntity
{
   public string FullName { get; set; } = null!;
   public DateTime DateOfBirth { get; set; }
   public Gender Gender { get; set; }
   public string MedicalHistory { get; set; } = null!;
   //public DateTime CreatedAt { get; set; }

   public ICollection<AefiReport> AefiReports { get; set; } = new List<AefiReport>();

}