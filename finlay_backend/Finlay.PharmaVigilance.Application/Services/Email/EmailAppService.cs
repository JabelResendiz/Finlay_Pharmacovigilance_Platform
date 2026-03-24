using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Application.IUnitOfWorkPattern;

namespace Finlay.PharmaVigilance.Application.Services;


public class EmailAppService : IEmailAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;

    public EmailAppService(IUnitOfWork unitOfWork, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    public async Task SendEmailToContactAsync(SendEmailDto dto)
    {
        var contact = await _unitOfWork.ContactRepository.GetByIdAsync(dto.ContactId);

        if (contact == null)
            throw new KeyNotFoundException($"Contacto con ID {dto.ContactId} no encontrado");

        if (!contact.IsActive)
            throw new InvalidOperationException("El contacto está inactivo");

        await _emailService.SendEmailAsync(contact.Email, dto.Subject, dto.Message);

    }
}