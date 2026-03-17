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

