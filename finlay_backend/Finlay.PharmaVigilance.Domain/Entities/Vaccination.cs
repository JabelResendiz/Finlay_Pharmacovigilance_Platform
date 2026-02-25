namespace Finlay.PharmaVigilance.Domain.Entities;


public class Vaccination : GenericEntity
{
    public string batchNumber {get;set;} = null!;
    public string administrationSite {get;set;} = null!;
    public int doseNumber {get;set;}
    public DateTime createdAt {get;set;}
    public DateTime administrationDate {get;set;}
}