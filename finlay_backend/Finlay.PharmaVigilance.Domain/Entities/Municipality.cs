namespace Finlay.PharmaVigilance.Domain.Entities;

public class Municipality
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    //FK
    public int ProvinceId { get; set; }
    public Province Province { get; set; } = null!;
}