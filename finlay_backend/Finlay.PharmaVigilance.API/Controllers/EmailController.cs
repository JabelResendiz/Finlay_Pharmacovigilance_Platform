using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;
using Finlay.PharmaVigilance.Application.Services.Email;
using Microsoft.AspNetCore.Mvc;

namespace Finlay.PharmaVigilance.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IContactCommandService _contactCommandService;

    public EmailController(
        IUnitOfWork unitOfWork,
        IEmailService emailService,
        IContactCommandService contactCommandService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _contactCommandService = contactCommandService;
    }

    /// <summary>
    /// Adds a new contact to the database
    /// </summary>
    [HttpPost("add-contact")]
    public async Task<IActionResult> AddContact([FromBody] CreateContactDto dto)
    {
        // if (!ModelState.IsValid)
        //     return BadRequest(ModelState);

        // // Validate email
        // if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains("@"))
        //     return BadRequest("Email inválido");

        // // Check if email already exists
        // var existingContact = await _unitOfWork.ContactRepository.GetByEmailAsync(dto.Email);
        // if (existingContact != null)
        //     return BadRequest("El correo electrónico ya existe en la base de datos");

        // var contact = new Contact
        // {
        //     Email = dto.Email.Trim().ToLower(),
        //     Name = dto.Name.Trim(),
        //     Phone = dto.Phone?.Trim(),
        //     Department = dto.Department?.Trim(),
        //     IsActive = true,
        //     CreatedAt = DateTime.UtcNow
        // };

        // await _unitOfWork.ContactRepository.CreateAsync(contact);
        // await _unitOfWork.CompleteAsync();

        // var result = new ContactDto
        // {
        //     Id = contact.Id,
        //     Email = contact.Email,
        //     Name = contact.Name,
        //     Phone = contact.Phone,
        //     Department = contact.Department,
        //     IsActive = contact.IsActive,
        //     CreatedAt = contact.CreatedAt
        // };

        var contact = await _contactCommandService.CreateAsync(dto);


        return CreatedAtAction(nameof(AddContact), new { id = contact.Id }, contact);
    }

    /// <summary>
    /// Sends an email to a specific contact
    /// </summary>
    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Get contact from database
        var contact = await _unitOfWork.ContactRepository.GetByIdAsync(dto.ContactId);
        if (contact == null)
            return NotFound($"Contacto con ID {dto.ContactId} no encontrado");

        if (!contact.IsActive)
            return BadRequest("El contacto está inactivo y no puede recibir emails");

        // Send email
        var success = await _emailService.SendEmailAsync(contact.Email, dto.Subject, dto.Message);

        if (!success)
            return StatusCode(500, "Error al enviar el email. Intente más tarde.");

        return Ok(new { message = "Email enviado exitosamente", email = contact.Email });
    }

    /// <summary>
    /// Gets all active contacts
    /// </summary>
    [HttpGet("contacts")]
    public async Task<IActionResult> GetContacts()
    {
        var contacts = await _unitOfWork.ContactRepository.GetActiveContactsAsync();
        var result = contacts.Select(c => new ContactDto
        {
            Id = c.Id,
            Email = c.Email,
            Name = c.Name,
            Phone = c.Phone,
            Department = c.Department,
            IsActive = c.IsActive,
            CreatedAt = c.CreatedAt
        }).ToList();

        return Ok(result);
    }
}
