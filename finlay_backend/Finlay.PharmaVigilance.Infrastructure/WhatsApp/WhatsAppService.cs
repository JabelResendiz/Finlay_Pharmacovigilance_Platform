using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Finlay.PharmaVigilance.Infrastructure.WhatsApp
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly string _apiBaseUrl;
        private readonly string _apiKey;
        private readonly string _sessionId;
        private readonly int _timeoutSeconds;
        private readonly HttpClient _httpClient;
        private readonly ILogger<WhatsAppService> _logger;

        public WhatsAppService(HttpClient httpClient, ILogger<WhatsAppService> logger, IOptions<WhatsAppSettings> options)
        {
            _httpClient = httpClient;
            _logger = logger;
            
            var settings = options.Value;
            _apiBaseUrl = settings.ApiBaseUrl;
            _apiKey = settings.ApiKey;
            _sessionId = settings.SessionId;
            _timeoutSeconds = settings.TimeoutSeconds;
            
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", _apiKey);
            _httpClient.Timeout = TimeSpan.FromSeconds(_timeoutSeconds);
        }

        public async Task<bool> SendMessageAsync(string sessionId, string phoneNumber, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sessionId))
                {
                    _logger.LogWarning("SendMessageAsync: sessionId no puede estar vacío");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    _logger.LogWarning("SendMessageAsync: phoneNumber no puede estar vacío");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    _logger.LogWarning("SendMessageAsync: message no puede estar vacío");
                    return false;
                }

                // Normalizar número de teléfono - remover caracteres especiales
                var cleanPhoneNumber = System.Text.RegularExpressions.Regex.Replace(phoneNumber, @"[^\d]", "");
                
                // Formato correcto para OpenWA
                var chatId = $"{cleanPhoneNumber}@c.us";
                
                _logger.LogDebug($"📱 Intentando enviar mensaje a {chatId} con sessionId: {sessionId}");
                
                var requestBody = new
                {
                    chatId = chatId,
                    text = message
                };

                var json = JsonSerializer.Serialize(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogDebug($"📤 Payload: {json}");

                // Endpoint correcto de OpenWA
                var endpoint = $"{_apiBaseUrl}/sessions/{sessionId}/messages/send-text";
                
                _logger.LogDebug($"🔗 Endpoint: {endpoint}");

                var response = await _httpClient.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation($"✅ Mensaje WhatsApp enviado exitosamente a {phoneNumber} en sesión {sessionId}. Response: {responseContent}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"❌ Error al enviar mensaje WhatsApp: {response.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "❌ Error de conexión al enviar mensaje WhatsApp");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error inesperado al enviar mensaje WhatsApp");
                return false;
            }
        }

        public async Task<bool> SendReportCreationConfirmationAsync(string phoneNumber, string notificationNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_sessionId))
                {
                    _logger.LogWarning("SendReportCreationConfirmationAsync: sessionId no puede estar vacío");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    _logger.LogWarning("SendReportCreationConfirmationAsync: phoneNumber no puede estar vacío");
                    return false;
                }

                if (string.IsNullOrWhiteSpace(notificationNumber))
                {
                    _logger.LogWarning("SendReportCreationConfirmationAsync: notificationNumber no puede estar vacío");
                    return false;
                }

                var message = $"✅ Reporte creado exitosamente.\n" +
                            $"Número de notificación: {notificationNumber}\n" +
                            $"Gracias por reportar.";

                return await SendMessageAsync(_sessionId, phoneNumber, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error al enviar confirmación de creación de reporte por WhatsApp");
                return false;
            }
        }
    }
}



