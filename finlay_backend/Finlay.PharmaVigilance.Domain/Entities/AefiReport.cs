namespace Finlay.PharmaVigilance.Domain.Entities;


public class AefiReport : GenericEntity
{
    public DateTime reportDate {get;set;}
    public string generalNotes {get;set;} = null!;
    public DateTime createdAt {get;set;}
    
}