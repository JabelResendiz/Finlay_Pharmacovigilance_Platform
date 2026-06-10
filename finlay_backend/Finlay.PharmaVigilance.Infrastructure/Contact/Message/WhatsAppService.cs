using System.Text;
using System.Text.Json;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Domain.Enum;
using Finlay.PharmaVigilance.Domain.Events;
using Finlay.PharmaVigilance.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Finlay.PharmaVigilance.Infrastructure.Email;

public class WhatsAppService : IMessageService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly WhatsAppSettings _settings;
    private readonly HttpClient _client;
    private readonly ILogger<WhatsAppService> _logger;


    public WhatsAppService(
        IHttpClientFactory httpClientFactory,
        IOptions<WhatsAppSettings> options,
        ILogger<WhatsAppService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _settings = options.Value;
        _logger = logger;

        _client = _httpClientFactory.CreateClient();

        _client.DefaultRequestHeaders.Add("X-API-Key", _settings.ApiKey);
        _client.Timeout = TimeSpan.FromSeconds(_settings.TimeoutSeconds);

    }

    public async Task SendEmailAsync<T>(
        string phoneNumber,
        EmailTemplateType templateType,
        T templateData) where T : BasicEvent
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_settings.SessionId))
            {
                _logger.LogWarning("SendMessageAsync: sessionId no puede estar vacío");
                return;
            }

            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                _logger.LogWarning("SendMessageAsync: phoneNumber no puede estar vacío");
                return;
            }


            string messageText;
            try
            {
                messageText = WhatsAppMessageFactory.Build(templateData);
            }
            catch (NotSupportedException ex)
            {
                _logger.LogWarning(ex, $"Evento {typeof(T).Name} no soportado para WhatsApp");
                return;
            }

            // Normalizar número de teléfono - remover caracteres especiales
            var cleanPhoneNumber = System.Text.RegularExpressions.Regex.Replace(phoneNumber, @"[^\d]", "");

            // Formato correcto para OpenWA
            var chatId = $"{cleanPhoneNumber}@c.us";

            var requestBody = new
            {
                chatId = chatId,
                text = messageText
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var endpoint = $"{_settings.ApiBaseUrl}/sessions/{_settings.SessionId}/messages/send-text";

            var response = await _client.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"✅ Mensaje WhatsApp enviado exitosamente a {phoneNumber} en sesión {_settings.SessionId}. Response: {responseContent}");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError($"❌ Error al enviar mensaje WhatsApp: {response.StatusCode} - {errorContent}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Error al enviar confirmación de creación de reporte por WhatsApp");
            return;
        }
    }

}