

namespace Finlay.PharmaVigilance.Domain.Entities;

public class Symptom : GenericEntity
{
    public string name {get;set;} = null!;
    public string description {get;set;} = null!;
    public string standardCode {get;set;} = null!;
    public DateTime createdAt {get;set;}
}