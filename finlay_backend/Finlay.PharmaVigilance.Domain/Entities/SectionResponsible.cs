namespace Finlay.PharmaVigilance.Domain.Entities;

public class SectionResponsible : GenericEntity
{
    //public string FullName { get; set; } = null!;
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}