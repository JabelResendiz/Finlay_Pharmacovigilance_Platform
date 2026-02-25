using Finlay.PharmaVigilance.Domain.Enum;

namespace Finlay.PharmaVigilance.Domain.Entities;

public class AdverseEvent : GenericEntity
{
    public DateTime startDate {get;set;}
    public string description {get;set;} = null!;
    public SeverityLevel severity {get;set;}
    public bool requiredHospitalization {get;set;}
    public string treatment {get;set;} = null!;
    public string notes {get;set;} = null!;
    public string currentStatus {get;set;} = null!;
    public DateTime createdAt {get;set;}

}