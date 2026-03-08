namespace Finlay.PharmaVigilance.Domain.Entities;


public class Vaccination : GenericEntity
{
    public string BatchNumber { get; set; } = null!;
    public string AdministrationSite { get; set; } = null!;
    public int DoseNumber { get; set; }
    //public DateTime CreatedAt { get; set; }
    public DateTime AdministrationDate { get; set; }


    public int VaccineId { get; set; }
    public Vaccine Vaccine { get; set; } = null!;
    public ICollection<AefiReport> AefiReports { get; set; } = new List<AefiReport>();
}