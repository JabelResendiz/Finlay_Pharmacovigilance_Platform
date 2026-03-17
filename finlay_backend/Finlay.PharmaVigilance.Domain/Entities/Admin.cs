namespace Finlay.PharmaVigilance.Domain.Entities;

public class Admin : GenericEntity
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}