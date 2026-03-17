namespace Finlay.PharmaVigilance.Domain.Entities;

public class Province
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
}