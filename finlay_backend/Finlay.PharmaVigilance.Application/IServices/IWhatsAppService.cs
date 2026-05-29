using System.Threading.Tasks;

namespace Finlay.PharmaVigilance.Application.IServices
{
    public interface IWhatsAppService
    {
        /// <summary>
        /// Envía un mensaje de texto a través de WhatsApp
        /// </summary>
        /// <param name="sessionId">ID de la sesión de WhatsApp</param>
        /// <param name="phoneNumber">Número de teléfono destinatario</param>
        /// <param name="message">Contenido del mensaje</param>
        /// <returns>True si el mensaje se envió exitosamente, False en caso contrario</returns>
        Task<bool> SendMessageAsync(string sessionId, string phoneNumber, string message);
    }
}
