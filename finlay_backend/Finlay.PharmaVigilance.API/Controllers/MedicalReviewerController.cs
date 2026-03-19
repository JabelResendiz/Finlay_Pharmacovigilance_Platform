using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.DTO.Authentication;
using Finlay.PharmaVigilance.Application.IServices.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicalReviewerController : ControllerBase
{

    public MedicalReviewerController()
    {
    }

    [HttpPost]
    [Route("register")]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RegisterMedicalReviewer(RegisterMedicalReviewerDto registerDto)
    {
        // var result = await _identityService.RegisterUserAsync(registerDto);

        // return Ok(new
        // {
        //     message = result
        // });

        return Ok(new
        {
            message = "Successful register medical reviewer"
        });
    }

    [HttpPost]
    [Route("login")]
    //[Authorize(Roles = "Supervisor")]
    public async Task<IActionResult> LoginMedicalReviewer(LoginMedicalReviewerDto loginDto)
    {

        // var token = await _identityService.LoginUserAsync(loginDto);
        return Ok(new
        {
            message = "Successful login medical reviewer"
        });

    }

}