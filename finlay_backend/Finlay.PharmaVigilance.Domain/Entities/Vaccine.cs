
namespace Finlay.PharmaVigilance.Domain.Entities;

public class Vaccine : GenericEntity
{
    public string name {get;set;} = null!;
    public string manufacturer {get;set;} = null!;
    public string vaccineType {get;set;} = null!;
    public string description {get;set;} = null!;
    public DateTime createdAt {get;set;}

}