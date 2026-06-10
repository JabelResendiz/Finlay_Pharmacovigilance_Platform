using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Finlay.PharmaVigilance.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WhatsappController : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService;

    public WhatsappController(IWhatsAppService whatsAppService)
    {
        _whatsAppService = whatsAppService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendText([FromBody] int phoneNumber)
    {
        
        var result = await _whatsAppService.SendReportCreationConfirmationAsync("5355664266","kdjfkshdgk");

        return StatusCode(StatusCodes.Status201Created, new
        {
            message = "Message sent successfully",
            data = result
        });
    }


   

}