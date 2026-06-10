using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;

namespace Finlay.PharmaVigilance.Application.IServices;

public interface IMessageService
{
    Task SendEmailAsync<T>(
        string phoneNumber,
        EmailTemplateType template,
        T templateData) where T : BasicEvent;
}

public interface IWhatsAppMessage
{
    string ToWhatsAppMessage();
}