using Microsoft.AspNetCore.Identity;

namespace Finlay.PharmaVigilance.Domain.Entities;


public class User: IdentityUser<int> {

}

public class Role : IdentityRole<int>{
    
}