
using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class Physician : GenericEntity{
   
   public string fullName {get;set;} = null!;
   public DateTime dateOfBirth {get;set;}
   public Gender gender {get;set;}
   public string medicalHistory {get;set;} = null!;
   public DateTime createdAt {get;set;}
   
}