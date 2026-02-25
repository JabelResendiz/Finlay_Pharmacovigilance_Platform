using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Patient : GenericEntity 
{
    public string fullName {get;set;} = null!;
    public string address {get;set;} = null!;
    public int age {get;set;}
    public DateTime dateOfBirth {get;set;}
    public Gender gender {get;set;}
    public Province Province {get;set;}
}