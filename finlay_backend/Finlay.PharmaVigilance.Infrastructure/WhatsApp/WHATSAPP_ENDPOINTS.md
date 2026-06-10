# Endpoints WhatsApp - OpenWA

## Configuración actual

- **Base URL**: `http://localhost:2785/api`
- **Session ID**: Se obtiene de `appsettings.json` (actualmente: `7769764c-b01d-4576-b6ce-bff47138b2c4`)
- **API Key**: Se envía en header `X-API-Key`

## Endpoint utilizado

```
POST http://localhost:2785/api/sessions/{sessionId}/messages/send-text
```

**Body:**
```json
{
  "chatId": "5491234567890@c.us",
  "text": "Tu mensaje aquí"
}
```

**Headers:**
```
X-API-Key: dev-admin-key
Content-Type: application/json
```

---

## Formato de número de teléfono

✅ Correcto:
- `5491234567890@c.us` (sin caracteres especiales)
- `+54 9 1234 567890` (se limpia automáticamente)
- `(549) 1234-567890` (se limpia automáticamente)

❌ Incorrecto:
- `54-912-345-67890`
- `+549-123-456-7890`

El servicio normaliza automáticamente el número eliminando todo excepto dígitos.

---

## Troubleshooting

Si los mensajes no se envían:

1. **Verifica que OpenWA esté corriendo en puerto 2785:**
   ```bash
   curl http://localhost:2785/api/sessions
   ```

2. **Verifica el SessionId:**
   ```bash
   curl -H "X-API-Key: dev-admin-key" http://localhost:2785/api/sessions
   ```

3. **Prueba manualmente con curl:**
   ```bash
   curl -X POST http://localhost:2785/api/sessions/7769764c-b01d-4576-b6ce-bff47138b2c4/messages/send-text \
     -H "Content-Type: application/json" \
     -H "X-API-Key: dev-admin-key" \
     -d '{
       "chatId": "5491234567890@c.us",
       "text": "Mensaje de prueba"
     }'
   ```

4. **Revisa los logs** en el backend por mensajes de error detallados

---

## Variables de Entorno

Para producción, usa variables de entorno:

```bash
WHATSAPP_API_BASE_URL=http://openwa-server:2785/api
WHATSAPP_API_KEY=tu-api-key-segura
WHATSAPP_SESSION_ID=tu-session-id-aqui
WHATSAPP_TIMEOUT_SECONDS=30
```

---

## Logging

El servicio genera logs detallados con emojis:
- 📱 Información del envío
- 📤 Payload enviado
- 🔗 Endpoint utilizado
- ✅ Éxito
- ❌ Error

Revisa los logs en la consola o archivo de logs del backend.
