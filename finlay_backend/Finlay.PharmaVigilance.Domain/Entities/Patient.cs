namespace Finlay.PharmaVigilance.Domain.Entities;

public class Patient : GenericEntity 
{
    public string Name {get;set;} = null!;
    public string Email {get;set;} = null!;
    public int PhoneNumber {get;set;}
    public string Address {get;set;} = null!;

}