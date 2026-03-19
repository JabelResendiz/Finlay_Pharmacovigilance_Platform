namespace Finlay.PharmaVigilance.Domain.Entities;

public class Province : GenericEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Municipality> Municipalities { get; set; } = new List<Municipality>();
}