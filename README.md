# Flujo del sistema de reporte de eventos adversos y manejo de duplicados

El sistema de farmacovigilancia tiene como objetivo permitir la notificación de **eventos adversos posteriores a la vacunación**, facilitando que tanto ciudadanos como profesionales de salud puedan reportar información relevante para su posterior análisis por parte de especialistas.

El diseño del sistema sigue principios utilizados por plataformas reales de farmacovigilancia como VAERS en Estados Unidos y el Yellow Card Scheme en el Reino Unido.

---

# Flujo general del sistema

El flujo de funcionamiento del sistema se divide en dos grandes etapas: **registro del reporte** y **evaluación del reporte**.

### 1. Registro del reporte

El proceso inicia cuando una persona detecta un posible evento adverso tras la administración de una vacuna.

Esta persona puede ser:

* el propio paciente
* un familiar o tutor
* un médico
* un profesional de enfermería
* cualquier ciudadano que tenga conocimiento del evento

El flujo es el siguiente:

```
Ciudadano o profesional de salud
            ↓
Completa formulario de reporte
            ↓
Registro del Reporte
            ↓
Base de datos del sistema
```

Durante el registro del reporte se recopilan diferentes bloques de información:

**Información del reportante**

* nombre (opcional según política de privacidad)
* correo electrónico o teléfono de contacto
* tipo de reportante (ciudadano, médico, familiar, etc.)
* relación con el sujeto vacunado

**Información del sujeto vacunado**

* edad
* sexo
* antecedentes médicos relevantes

**Información de la vacuna**

* tipo de vacuna
* fecha de vacunación
* número de lote (si está disponible)

**Información del evento adverso**

* síntomas observados
* fecha de inicio
* evolución del evento
* descripción narrativa del caso

Una vez enviado el formulario, el sistema genera un **identificador único de reporte**, que permite posteriormente rastrear el caso dentro de la base de datos.


---

# FLujo General del Sistema(propuesta integrada)

El sistema opera mediante un modelo de notificación colaborativa, donde el reporte inicial se fortalece con intervención profesional.

## Registro del Reporte (Fase de Captura)
El proceso inicia cuando un ciudadano o profesional detecta una sospecha de reacción adversa. No es necesaria la certeza, basta con la sospecha para notificar
1. Actores: Paciente, familiar, médico, enfermero o cualquier ciudadano.
2. Bloques de Información:
   - Notificador: Nombre, contacto y relación con el sujeto
   - Sujeto Vacunado: Edad, sexo y antecedentes médicos
   - Datos de Ubicación (Clave para Alertas): Se añade Provincia y Municipio de residencia y el Lugar de vacunación (escuela, centro móvil, hospital)
   - Vacuna (Simplificada para el Ciudadano): Nombre de la vacuna y fecha. El número de lote es opcional si el ciudadano no lo recuerda
   - Evento Adverso: Fecha de inicio, síntomas (descripción narrativa o selección simple) y evolución

## Sistema de Alerta y Asignación Municipal
Una vez enviado el formulario, el sistema genera un identificador único de reporte (número de referencia)
 y activa el siguiente mecanismo:
1. **Geolocalización de la Alerta:** El sistema analiza el Municipio declarado por el ciudadano.
2. **Disparo de Alerta:** Se envía una notificación automática al Área de Salud o Policlínico correspondiente a la dirección del ciudadano.
3. **Priorización:** Si el reporte indica que el evento es Grave (requirió hospitalización o atención médica), la alerta se marca como urgente

## Evaluación y Fortalecimiento (Fase Médica)
El médico asignado en el municipio recibe la alerta y accede al reporte mediante el identificador único. Su función es validar y profesionalizar el dato:
- **Contacto Clínico:** El médico contacta al reportante para discutir detalles técnicos
- **Completamiento Técnico:** El médico busca en los registros del centro de vacunación (incluso si fue en otro municipio) para introducir el número de lote y fabricante exactos, datos vitales para la seguridad biológica
- **Normalización MedDRA:** El médico traduce los síntomas narrativos del ciudadano a términos médicos estandarizados (MedDRA) para asegurar la calidad del dato y su futura importación a sistemas nacionales
- **Ampliación:** El profesional añade resultados de pruebas de laboratorio o mediciones clínicas (presión, pulso) que el ciudadano desconoce

---

# Interfaz Diferenciada por Perfil de Usuario
El sistema adapta dinámicamente la complejidad del formulario basándose en si el notificador se identifica como ciudadano o como profesional sanitario, optimizando la calidad del dato recolectado

## Vista para Ciudadanos (No profesionales)
- **Lenguaje Coloquial:** El formulario presenta los síntomas con descripciones sencillas (ej: "dolor de barriga" para dolor abdominal o "tupición en la nariz" para congestión nasal) para facilitar la comprensión
- **Flexibilidad en Datos Técnicos:** Campos como el número de lote, el fabricante o el nombre comercial de la vacuna se marcan como opcionales ("si lo conoce"), reconociendo que el ciudadano puede no disponer de esa información al momento
- **Narrativa Libre:** Se prioriza una descripción breve y narrativa de cómo ocurrió el evento

## Vista para Profesionales de la Salud que Crear Reporte
- **Información Esencial y No Sustituible:** Numero de identidad del profesional, ademas de informacion de contacto y otros campos especificos del medico.
- **Estandarización MedDRA:** Se habilita el uso obligatorio de términos técnicos MedDRA (LLT/PT) mediante autocompletado para asegurar que el diagnóstico sea preciso y pueda integrarse a sistemas nacionales
- **Detalle Clínico Avanzado:** Se activan secciones para reportar mediciones clínicas anormales (temperatura, pulso, presión arterial)
, resultados bioquímicos o analíticos (como biopsias o concentraciones plasmáticas)
 y detalles sobre tratamientos instaurados para mitigar la reacción adversa

El proceso no quita que se active el proceso de alerta, porque incluso si el autorreporte lo hace un profesional tiene que verificar que sea producto de la vacunación.

---

# Numero de Referencia y Seguimiento

Para permitir que el sistema sea accesible sin necesidad de un registro previo obligatorio para todos los usuarios, se implementa un mecanismo de seguimiento basado en un identificador único.

- **Acuse de Recibo:** Una vez enviado el reporte inicial, el sistema genera automáticamente un informe de acuse de recibo que se envía al notificador
- **Vínculo de Actualización:** Este informe incluye un número de referencia único, el cual es la llave para que el usuario pueda modificar o ampliar la información ya enviada en el futuro
- **Seguridad y Trazabilidad:** El uso de este número permite adjuntar nuevos datos clínicos o resultados de laboratorio a una notificación existente sin necesidad de crear un nuevo registro, evitando duplicados y manteniendo la integridad del caso

---

# Revisión y análisis de los reportes

Después del registro, el reporte entra en una fase de evaluación dentro del sistema.

En esta etapa intervienen usuarios internos que sí requieren autenticación, tales como:

* revisores médicos
* responsables de secciones o regiones
* administradores del sistema

El flujo interno es el siguiente:

```
Reporte recibido
        ↓
Validaciones automáticas
        ↓
Detección de posibles duplicados
        ↓
Revisión por especialista
        ↓
Clasificación del caso
```

Estos usuarios tienen roles dentro del sistema que les permiten revisar, validar y analizar la información reportada.

---

# Manejo de reportes duplicados

En sistemas abiertos de farmacovigilancia es común que un mismo evento sea reportado varias veces. Esto puede ocurrir, por ejemplo, cuando:

* el paciente reporta el evento
* un familiar también lo reporta
* un médico envía otro reporte sobre el mismo caso

Por esta razón, sistemas como VAERS no intentan eliminar completamente los duplicados, sino **detectarlos y gestionarlos posteriormente**.

El proceso suele incluir varias capas de control.

## Validaciones automáticas

Al momento de registrar el reporte el sistema valida:

* campos obligatorios
* coherencia entre fechas
* valores plausibles de edad
* formatos de correo o teléfono

Estas validaciones reducen errores comunes de captura de datos.

## Identificación de posibles duplicados

Posteriormente el sistema puede ejecutar algoritmos de comparación entre reportes utilizando variables como:

* edad del paciente
* sexo
* fecha de vacunación
* tipo de vacuna
* fecha de inicio de los síntomas
* ubicación geográfica

Si varios campos coinciden entre dos reportes, el sistema puede marcar el caso como **posible duplicado**.

## Revisión por especialistas

Los reportes marcados como posibles duplicados son revisados por especialistas del sistema, quienes analizan la información clínica y la narrativa del evento para determinar si se trata realmente del mismo caso o de eventos diferentes.

Este proceso permite consolidar reportes relacionados o mantenerlos separados cuando corresponde.

---

# Autenticación de reportantes

Un aspecto importante del diseño del sistema es decidir si los reportantes deben o no autenticarse mediante una cuenta de usuario.

Existen dos enfoques principales.

## Reportes sin autenticación (modelo abierto)

En este modelo cualquier ciudadano puede enviar un reporte sin necesidad de crear una cuenta.

Ventajas:

* reduce barreras para reportar
* aumenta la cantidad de reportes recibidos
* facilita la participación ciudadana
* es el modelo utilizado por varios sistemas internacionales

Desventajas:

* mayor probabilidad de reportes duplicados
* mayor dificultad para validar identidad del reportante
* posibilidad de reportes incorrectos o incompletos

## Reportes con autenticación (modelo con cuenta)

En este enfoque el reportante debe registrarse e iniciar sesión antes de enviar un reporte.

Ventajas:

* mayor trazabilidad de quién envía el reporte
* facilita el seguimiento de casos
* reduce algunos tipos de fraude o spam
* permite al usuario consultar sus reportes enviados

Desventajas:

* aumenta la fricción para el usuario
* puede reducir la cantidad de reportes recibidos
* requiere gestión de cuentas y seguridad adicional

---

# Enfoque recomendado

Muchos sistemas modernos utilizan un **modelo híbrido**, en el cual:

* los ciudadanos pueden reportar sin autenticarse
* los especialistas del sistema sí deben autenticarse para revisar los casos

Esto permite maximizar la cantidad de reportes recibidos mientras se mantiene control institucional sobre la evaluación de los eventos adversos.

---


# Autenticación y gestión de usuarios (médicos y responsables)

Un aspecto clave del sistema es definir **cómo se crean y acceden los usuarios internos**, específicamente:

* médicos (validan y completan reportes)
* responsables de sección (gestionan médicos por área)
* administradores (gestionan el sistema)

Dado que se trata de un **sistema institucional cerrado**, no se permite registro público.
Los usuarios son creados por niveles superiores dentro de la organización.

---

## Modelos de creación y autenticación de usuarios

### 1. Creación por responsable con contraseña temporal (modelo recomendado)

En este modelo, un usuario superior crea las cuentas de los usuarios subordinados.

#### Flujo

1. El responsable crea un médico en el sistema:

   * username (puede ser CI o generado)
   * contraseña temporal (ej: `Medico#1234`)
   * `MustChangePassword = true`

2. El responsable comunica las credenciales:

   * en persona
   * por vía institucional interna

3. El médico inicia sesión:

   * introduce usuario + contraseña temporal

4. El sistema detecta que debe cambiar la contraseña:

   * redirige automáticamente a cambio de contraseña

5. El médico define su contraseña definitiva

---

#### Ventajas

* no depende de correo electrónico
* funciona en entornos con baja conectividad
* control total por parte de la institución
* flujo utilizado en sistemas hospitalarios y gubernamentales

---

#### Desventajas

* requiere comunicación manual de credenciales
* depende del responsable para la creación inicial

---

### 2. Contraseña inicial basada en CI (modelo práctico)

En este modelo se utiliza el CI como base de autenticación inicial.

#### Flujo

1. El responsable crea el usuario:

   * username = CI
   * contraseña inicial = CI o parte del CI
   * `MustChangePassword = true`

2. El médico inicia sesión con esos datos

3. El sistema obliga a cambiar la contraseña

---

#### Ventajas

* no requiere enviar credenciales
* fácil de recordar para el usuario
* muy práctico en entornos institucionales

---

#### Desventajas

* menor seguridad inicial
* requiere forzar cambio de contraseña obligatoriamente

---

### 3. Activación mediante enlace (modelo moderno)

En este modelo el usuario define su contraseña mediante un enlace de activación.

#### Flujo

1. El responsable crea el usuario sin contraseña

2. El sistema genera un token de activación:

   ```go
   /activate-account?token=xyz
   ```

3. El usuario recibe el enlace (correo u otro medio)

4. El usuario accede al enlace y define su contraseña

---

#### Ventajas

* mayor seguridad
* el usuario crea su propia contraseña
* evita compartir credenciales

---

#### Desventajas

* depende de correo electrónico o mensajería
* no siempre viable en entornos con baja conectividad (ej: Cuba)

---

## Gestión por niveles de usuarios

### Administradores

Los administradores son pocos usuarios (generalmente 2 o 3) y tienen control total del sistema.

#### Flujo

1. Las cuentas se crean manualmente:

   * directamente en la base de datos o por script

2. Se asigna una contraseña segura:

   * generada manualmente

3. Las credenciales se entregan por vía segura:

   * en persona
   * canal interno confiable

4. Se recomienda cambio de contraseña en primer acceso

---

#### Ventajas

* máximo control
* alta seguridad
* adecuado para pocos usuarios críticos

---

## Recomendación final del sistema

Para un sistema institucional como este:

✔ Administradores
→ creación manual + credenciales seguras

✔ Responsables
→ contraseña temporal + cambio obligatorio

✔ Médicos
→ contraseña temporal o CI + cambio obligatorio

❌ No usar autenticación sin contraseña

---

# Email alerta:

## Objetivo

Explicar cómo diseñar e implementar un sistema de alertas por correo electrónico que, a partir de un reporte creado en la plataforma, notifique a un médico o responsable de un área de salud determinada sobre un sujeto vacunado usando el email provisto por los usuarios.

## Requisitos funcionales

- Usar el correo suministrado por los usuarios para notificaciones.
- Permitir configurar destinatarios por área de salud y rol (ej. médico de zona X).
- Generar y enviar notificaciones al momento de crear un reporte relevante.
- Garantizar entrega fiable (reintentos, encolado) y registro (logs).

## Requisitos no funcionales

- Seguridad y privacidad: cumplir GDPR/leyes locales, opt-in/consentimiento.
- Escalabilidad: soportar picos de reportes (colas y workers).
- Observabilidad: métricas, trazas y alertas sobre fallos de envío.

## Arquitectura propuesta (visión general)

- Frontend / API: recibe el reporte y lo persiste en la base de datos.
- Capa de negocio: publica un evento `ReportCreated` al crear el reporte.
- Servicio/Worker de notificaciones (background): suscrito a eventos, genera el email y lo envía.
- Cola de mensajes (opcional, recomendado): RabbitMQ, Azure Service Bus, Redis Streams para desacoplar y permitir reintentos.
- Servicio de email/SMTP: SMTP propio o proveedor (SendGrid, Mailgun, Amazon SES).
- Almacenamiento: tabla para `EmailQueue` / logs de envío y plantilla de email.

Diagrama simplificado de flujo:

1. Usuario crea reporte → 2. API guarda reporte → 3. Publica evento `ReportCreated` → 4. Worker toma evento → 5. Consulta destinatarios/plantilla → 6. Encola email → 7. Servicio de envío procesa la cola y entrega el email → 8. Log / métricas

## Cambios en el modelo de datos (sugeridos)

- Tabla `Reports` (existente) — sin cambios obligatorios.
- Tabla `AreaContacts` o `HealthAreaRecipients`:
  - `Id` (PK)
  - `AreaCode` / `AreaId`
  - `Role` (ej. `MEDICO_RESPONSABLE`)
  - `Email` (destinatario)
  - `Name` (opcional)
  - `IsActive`, `CreatedAt`
- Tabla `EmailQueue` (para auditoría/reintentos):
  - `Id`, `ToEmail`, `Subject`, `Body`, `Status` (Pending/Sent/Failed), `Attempts`, `LastError`, `CreatedAt`, `NextAttemptAt`

También guardar consentimientos si el email pertenece a un usuario y se requiere permiso explícito.

## Integración con la creación de reportes

- En el flujo donde se crea el reporte (por ejemplo `ReportController.Create`):
  1. Persistir el reporte en BD.
  2. Identificar el área de salud y los contactos activos para esa área (leer `AreaContacts`).
  3. Publicar un evento `ReportCreated` con datos mínimos: `ReportId`, `AreaId`, `VaccinatedSubjectId`, `Severity`, `Timestamp`.
  4. Responder al usuario (no bloquear por envío de email).

El worker de notificaciones se suscribe a `ReportCreated` y realiza la lógica de envío.

## Diseño del worker de notificaciones

- Responsabilidades:
  - Recibir eventos (o leer la `EmailQueue`).
  - Generar contenido del correo (plantilla) con datos del reporte.
  - Validar dirección de correo.
  - Encolar y/o enviar el email.
  - Registrar el resultado y realizar reintentos con backoff.

- Componentes:
  - `IEmailSender` (interfaz) — método `SendAsync(EmailMessage)`.
  - Implementación SMTP/proveedor: `SmtpEmailSender`, `SendGridEmailSender`.
  - `NotificationWorker` (background service o Hangfire job) que procesa la cola.

## Estrategia de fiabilidad y reintentos

- Al enviar, usar patrón de encolado con estados:
  - `Pending` → `Processing` → `Sent` o `Failed`.
- Reintentos exponenciales: 3–5 intentos con backoff (ej. 1m, 5m, 20m, 1h).
- Registrar `LastError` y, si excede intentos, marcar `Failed` y crear una alerta operacional.

## Seguridad y cumplimiento

- Validar emails y evitar inyección en plantillas.
- Almacenar credenciales SMTP/proveedor en variables de entorno (`SMTP_HOST`, `SMTP_USER`, `SMTP_PASS`, `SENDGRID_API_KEY`).
- Asegurarse del consentimiento del propietario del email si la legislación lo exige.
- Cifrar datos sensibles en reposo si necesario (emails personales o historiales).

## Plantillas de correo y personalización

- Usar un motor de plantillas (RazorLight, Liquid, Handlebars) para separar lógica y presentación.
- Mantener plantillas en BD o en archivos versionados.
- Ejemplo de campos en la plantilla:
  - Nombre del médico, ID del reporte, fecha, datos mínimos del sujeto vacunado (sin PHI excesiva), enlace seguro al reporte en la plataforma.

Ejemplo simple de asunto y cuerpo:

Asunto: "Nuevo reporte de sujeto vacunado — Report #{ReportId}"

Cuerpo (resumido):

"Estimado Dr/a {RecipientName},\n\nSe ha creado un nuevo reporte (ID {ReportId}) para el área {AreaName}. Datos relevantes: Fecha: {Date}, Edad: {Age}, Estado: {Severity}.\n\nVer reporte: https://mi-plataforma/reports/{ReportId}\n\nAtentamente,\nEquipo de Farmacovigilancia"

## Ejemplo de pseudocódigo (C#) — worker simplificado

```csharp
public class NotificationWorker : BackgroundService
{
    private readonly IEmailSender _emailSender;
    private readonly IAreaRepository _areaRepo;
    private readonly IEmailQueueRepository _queueRepo;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            var job = await _queueRepo.DequeuePendingAsync(); // bloqueante/long-polling
            if (job == null) { await Task.Delay(1000); continue; }

            try
            {
                await _emailSender.SendAsync(new EmailMessage(job.ToEmail, job.Subject, job.Body));
                job.MarkSent();
            }
            catch(Exception ex)
            {
                job.IncrementAttempts(ex.Message);
                if (job.AttemptsExceeded) job.MarkFailed();
            }
            await _queueRepo.UpdateAsync(job);
        }
    }
}
```

Y el código que publica la tarea de email cuando se crea el reporte:

```csharp
// En ReportService después de guardar reporte
var recipients = _areaRepo.GetRecipients(report.AreaId);
var template = _templateService.Render("new-report", model);
foreach(var r in recipients){
    _queueRepo.Enqueue(new EmailQueueItem { ToEmail = r.Email, Subject = subject, Body = template });
}
```

## Configuración recomendada (variables de entorno)

- `SMTP_HOST`, `SMTP_PORT`, `SMTP_USER`, `SMTP_PASS`
- `EMAIL_FROM_NAME`, `EMAIL_FROM_ADDRESS`
- `QUEUE_TYPE` (db/rabbit/azure)
- `MAX_EMAIL_ATTEMPTS`, `EMAIL_RETRY_BASE_SECONDS`

## Monitorización y observabilidad

- Exponer métricas: emails enviados, fallidos, latencia, tamaño de cola.
- Logs estructurados con `ReportId`, `RecipientEmail`, `Status`.
- Integrar con sistema de alertas (PagerDuty/Teams/Slack) si la cola crece o hay muchas fallas.

## Pruebas

- Unit tests para: renderizado de plantillas, validación de emails, política de reintentos.
- Integration tests: usar proveedor de email de sandbox (SendGrid Sandbox, Amazon SES sandbox) o un servidor SMTP de prueba.
- End-to-end: crear reporte en ambiente de staging y verificar que la cola y el worker entregan el correo.

## Consideraciones legales y de privacidad

- Minimizar PHI en email; si se envía información sensible, usar enlaces seguros que requieran autenticación.
- Registrar consentimiento para comunicaciones cuando aplique.

## Puntos de decisión / alternativas

- Encolado vs envío en línea: encolado recomendado (no bloquear el request del usuario).
- SMTP propio vs proveedor: proveedores facilitan entrega (rechazos, reputación, templates), pero añaden coste.
- Retry en worker vs reintentos del proveedor: combinar ambos para mayor resiliencia.

## Próximos pasos de implementación

1. Añadir la tabla `AreaContacts` y `EmailQueue` en la BD.
2. Implementar `IEmailSender` con proveedor elegido.
3. Publicar evento `ReportCreated` al guardar reportes.
4. Implementar `NotificationWorker` que consuma la cola/eventos.
5. Agregar pruebas y despliegue en staging.

---

# 📄 PDF Editable

## 🧠 Descripción

El sistema permite la generación y uso de un **formulario en formato PDF editable (AcroForm)** para el registro de reportes de eventos adversos.

El usuario puede:

* Descargar un formulario PDF oficial
* Completarlo de forma offline en su computadora
* Subirlo nuevamente al sistema para su procesamiento

Este enfoque permite **accesibilidad sin autenticación** y recolección estandarizada de datos.

---

## 🔄 Flujo del sistema

```text
1. Usuario accede al sistema público
2. Descarga PDF editable
3. Completa el formulario offline
4. Sube el PDF al sistema
5. Backend valida y procesa el archivo
6. Se genera un ID de caso (tracking)
```

---

## 🧾 Estructura del PDF

El PDF contiene campos editables organizados en secciones:

### 🏥 Datos del reportante

* Nombre del reportante
* Email
* Teléfono / contacto
* CI del reportante

---

### 💉 Datos del sujeto vacunado

* Nombre completo
* CI
* Provincia
* Municipio
* Área de salud

---

### 🧪 Datos de la vacuna

* Tipo de vacuna
* Fecha de administración
* Lote (si aplica)

---

### ⚠️ Evento adverso

* Descripción de síntomas (campo multilinea)
* Fecha de aparición
* Severidad

---

### ✍️ Firma

* Firma del reportante
* Fecha

---

## 🔐 Seguridad del sistema

Debido a que el sistema es público (sin login), se implementan medidas de seguridad:

### 🛡️ Validación de archivos

* Solo se aceptan archivos PDF
* Tamaño máximo permitido
* Verificación de estructura PDF válida

---

### 🤖 Protección anti-bots

* CAPTCHA en el formulario de subida
* Rate limiting por IP para evitar abuso

---

### 🦠 Escaneo de archivos

* Escaneo antivirus antes de procesar el archivo (ej: ClamAV)
* Rechazo de archivos corruptos o inválidos

---

### 🧾 Integridad del sistema

* Generación de ID único de caso (tracking number)
* El PDF es tratado como **documento de evidencia**, no como fuente confiable de datos

---

## ⚠️ Consideraciones importantes

* El contenido del PDF **NO se considera fuente de verdad**
* Los datos pueden ser revisados manualmente
* El sistema prioriza accesibilidad sobre autenticación
* Se recomienda complementar con un sistema autenticado para personal médico

---

## 🧱 Tecnologías sugeridas

* PDF editable (AcroForm)

  * iText7
  * PdfSharp

* Antivirus:

  * ClamAV

* Protección API:

  * Rate Limiting (ASP.NET Middleware)
  * CAPTCHA (reCAPTCHA)

---

## 🎯 Objetivo

Proveer un mecanismo accesible para el reporte de eventos adversos mediante un **formulario PDF estándar editable**, garantizando:

* facilidad de uso
* compatibilidad offline
* trazabilidad mediante ID de caso
* seguridad básica contra abuso

---

