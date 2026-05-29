using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Finlay.PharmaVigilance.Application.IServices;
using Finlay.PharmaVigilance.Infrastructure.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Finlay.PharmaVigilance.Infrastructure.WhatsApp
{
    public class WhatsAppService : IWhatsAppService
    {
        private readonly string _apiBaseUrl;
        private readonly string _apiKey;
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

                var chatId = $"{phoneNumber}@c.us"; // Formato WhatsApp
                
                var requestBody = new
                {
                    chatId = chatId,
                    text = message,
                    options = new { }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(
                    $"{_apiBaseUrl}/sessions/{sessionId}/messages/send-text",
                    content
                );

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation($"Mensaje WhatsApp enviado exitosamente a {phoneNumber} en sesión {sessionId}");
                    return true;
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error al enviar mensaje WhatsApp: {response.StatusCode} - {errorContent}");
                    return false;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión al enviar mensaje WhatsApp");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al enviar mensaje WhatsApp");
                return false;
            }
        }
    }
}
