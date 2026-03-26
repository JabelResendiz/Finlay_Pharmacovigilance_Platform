using Finlay.PharmaVigilance.Application.DTO;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IEmailAppService
{
    Task SendEmailToContactAsync(SendEmailDto dto);
}