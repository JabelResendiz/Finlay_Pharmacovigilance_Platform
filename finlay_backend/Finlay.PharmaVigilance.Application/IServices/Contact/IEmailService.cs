using Finlay.PharmaVigilance.Application.DTO;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IEmailService
{
    Task SendEmailAsync<T>(
        string toEmail,
        EmailTemplateType template,
        T templateData) where T : IBasicTemplate;
}