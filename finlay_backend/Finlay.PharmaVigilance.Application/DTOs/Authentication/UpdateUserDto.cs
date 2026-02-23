
namespace Finlay.PharmaVigilance.Application.DTO.Authentication;

public class UpdateUserDto
{
    public int Id {get;set;}
    public string UserRole { get; set; } = null!;
    public string Name {get;set;} = null!;
    public string UserName {get;set;} = null!;
    public string Email {get;set;} = null!;
    
}