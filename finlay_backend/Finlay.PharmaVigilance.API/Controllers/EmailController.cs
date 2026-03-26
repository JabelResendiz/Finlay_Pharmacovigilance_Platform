using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IContactCommandService _contactCommandService;
    private readonly IContactQueryService _contactQueryService;
    private readonly IEmailAppService _emailAppService;

    public EmailController(
        IEmailAppService emailAppService,
        IContactCommandService contactCommandService,
        IContactQueryService contactQueryService)
    {
        _emailAppService = emailAppService;
        _contactCommandService = contactCommandService;
        _contactQueryService = contactQueryService;
    }

    /// <summary>
    /// Adds a new contact to the database
    /// </summary>
    [HttpPost("add-contact")]
    public async Task<IActionResult> AddContact([FromBody] CreateContactDto dto)
    {
        var contact = await _contactCommandService.CreateAsync(dto);

        return CreatedAtAction(nameof(AddContact), new { id = contact.Id }, contact);
    }

    /// <summary>
    /// Sends an email to a specific contact
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailDto dto)
    {
        await _emailAppService.SendEmailToContactAsync(dto);

        return Ok(new { message = "Successfully sent email to contact" });

    }

    /// <summary>
    /// Gets all active contacts
    /// </summary>
    [HttpGet("contacts")]
    public async Task<IActionResult> GetContacts()
    {

        var result = await _contactQueryService.GetActiveContactsAsync();

        return Ok(result);
    }
}
