# Servicio WhatsApp

## Descripción
El servicio `IWhatsAppService` proporciona funcionalidad para enviar mensajes de texto a través de WhatsApp utilizando la API de WhatsApp Web (via Whatsapp Web.js o similar).

## Instalación y Configuración

### 1. Inyección de Dependencias
El servicio se registra automáticamente en la clase `DependencyInjection.cs` de la capa de Infrastructure.

### 2. Configuración en `appsettings.json`
Añade la siguiente configuración a tu archivo `appsettings.json`:

```json
{
  "WhatsApp": {
    "ApiBaseUrl": "http://localhost:2785/api",
    "ApiKey": "dev-admin-key",
    "TimeoutSeconds": 30
  }
}
```

Para ambientes diferentes (Development, Production), puedes usar `appsettings.Development.json` o `appsettings.Production.json`:

```json
{
  "WhatsApp": {
    "ApiBaseUrl": "https://tu-servidor-whatsapp.com/api",
    "ApiKey": "tu-api-key-produccion",
    "TimeoutSeconds": 60
  }
}
```

## Uso

### En un Controlador
```csharp
using Finlay.PharmaVigilance.Application.IServices;

[ApiController]
[Route("api/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IWhatsAppService _whatsAppService;

    public NotificationController(IWhatsAppService whatsAppService)
    {
        _whatsAppService = whatsAppService;
    }

    [HttpPost("send-whatsapp")]
    public async Task<IActionResult> SendWhatsAppMessage([FromBody] SendWhatsAppRequest request)
    {
        var result = await _whatsAppService.SendMessageAsync(
            request.SessionId,
            request.PhoneNumber,
            request.Message
        );

        if (result)
            return Ok(new { success = true, message = "Mensaje enviado exitosamente" });
        
        return BadRequest(new { success = false, message = "Error al enviar el mensaje" });
    }
}

public class SendWhatsAppRequest
{
    public string SessionId { get; set; }
    public string PhoneNumber { get; set; }
    public string Message { get; set; }
}
```

### En un Servicio de Aplicación
```csharp
using Finlay.PharmaVigilance.Application.IServices;

public class ReportNotificationService
{
    private readonly IWhatsAppService _whatsAppService;

    public ReportNotificationService(IWhatsAppService whatsAppService)
    {
        _whatsAppService = whatsAppService;
    }

    public async Task NotifyReportSubmissionAsync(string phoneNumber, string reportId)
    {
        var message = $"Su reporte #{reportId} ha sido recibido exitosamente.";
        await _whatsAppService.SendMessageAsync("main-session", phoneNumber, message);
    }
}
```

## Métodos Disponibles

### SendMessageAsync
```csharp
/// <summary>
/// Envía un mensaje de texto a través de WhatsApp
/// </summary>
/// <param name="sessionId">ID de la sesión de WhatsApp</param>
/// <param name="phoneNumber">Número de teléfono destinatario (ej: 5511987654321)</param>
/// <param name="message">Contenido del mensaje</param>
/// <returns>True si el mensaje se envió exitosamente, False en caso contrario</returns>
public async Task<bool> SendMessageAsync(string sessionId, string phoneNumber, string message)
```

## Validación de Entrada
El servicio valida automáticamente:
- `sessionId` no puede estar vacío
- `phoneNumber` no puede estar vacío
- `message` no puede estar vacío

## Logging
El servicio proporciona logs en diferentes niveles:
- **Information**: Cuando un mensaje se envía exitosamente
- **Warning**: Cuando hay validación de entrada fallida
- **Error**: Cuando hay errores de conexión o respuestas no exitosas

## Manejo de Errores
- **HttpRequestException**: Se captura cuando hay problemas de conectividad con el API
- **Timeout**: Se maneja automáticamente basado en `TimeoutSeconds` en configuración
- **General Exception**: Cualquier otro error se registra en logs

## Requisitos
- Servidor WhatsApp Web API disponible (en `http://localhost:2785/api` por defecto)
- API Key válida configurada
- HttpClient registrado en dependency injection
- Logger configurado en la aplicación

## Notas Importantes
- El número de teléfono debe incluir el código de país (ej: 5511987654321 para Brasil)
- El formato de WhatsApp utiliza el sufijo `@c.us` automáticamente
- La sesión debe estar activa en el servidor WhatsApp
- Los mensajes se envían de forma asincrónica
