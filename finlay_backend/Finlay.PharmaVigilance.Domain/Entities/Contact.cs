namespace Finlay.PharmaVigilance.Domain.Entities;

public class Contact : GenericEntity
{
    public string Email { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Department { get; set; }
    public bool IsActive { get; set; } = true;
}
