namespace Finlay.PharmaVigilance.Domain.Entities;

public class Municipality : GenericEntity
{
    public string Name { get; set; } = null!;

    //FK
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
}