using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AuthenticationController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    // [HttpPost]
    // [Route("register/medical_reviewer")]
    // //[Authorize(Roles = "Administrator")]
    // public async Task<IActionResult> RegisterMedicalReviewer(RegisterMedicalReviewerDto registerDto)
    // {
    //     var result = await _identityService.RegisterUserAsync(registerDto);

    //     return Ok(new
    //     {
    //         message = result
    //     });
    // }

    [HttpPost]
    [Route("login")]
    //[Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> LoginUser(LoginUserDto loginDto)
    {

        var token = await _identityService.LoginUserAsync(loginDto);
        return Ok(token);

    }

}