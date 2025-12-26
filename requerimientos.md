
# 📌 Contexto del sistema

El sistema propone una **plataforma web y móvil** para el **reporte, gestión y análisis de eventos adversos posteriores a la vacunación**, orientada al Instituto Finlay de Vacunas. Permitirá a **ciudadanos y profesionales de la salud** registrar reportes, mientras que **autoridades sanitarias** podrán analizarlos para apoyar la **farmacovigilancia** y la toma de decisiones.

---

# ✅ Requerimientos Funcionales (RF)

### RF-01 Registro de usuarios

El sistema deberá permitir el **registro de usuarios** bajo los siguientes roles:

* Ciudadano
* Profesional de la salud
* Administrador sanitario

---

### RF-02 Autenticación y autorización

El sistema deberá permitir:

* Inicio de sesión seguro
* Control de acceso basado en roles (RBAC)

---

### RF-03 Reporte de evento adverso

El sistema deberá permitir a los usuarios registrar un evento adverso asociado a una vacuna, incluyendo:

* Datos demográficos del paciente (anonimizados)
* Vacuna administrada (tipo, lote, fecha)
* Síntomas presentados
* Gravedad del evento
* Evolución del paciente
* Evidencia adicional (opcional)

---

### RF-04 Edición y seguimiento del reporte

El sistema deberá permitir:

* Editar reportes en estado “pendiente”
* Consultar el estado del reporte (en análisis, validado, descartado)

---

### RF-05 Validación médica de reportes

El sistema deberá permitir a profesionales de la salud:

* Revisar reportes
* Clasificarlos según criterios clínicos
* Marcar inconsistencias o duplicados

---

### RF-06 Gestión administrativa

El sistema deberá permitir a administradores:

* Aprobar o rechazar reportes
* Gestionar usuarios
* Configurar catálogos (vacunas, síntomas, eventos)

---

### RF-07 Análisis y visualización de datos

El sistema deberá permitir:

* Visualización de estadísticas agregadas
* Filtros por vacuna, fecha, gravedad, región
* Exportación de datos anonimizados

---

### RF-08 Notificaciones

El sistema deberá notificar:

* Al usuario sobre cambios en su reporte
* A autoridades sanitarias sobre eventos críticos

---

### RF-09 Interoperabilidad

El sistema deberá permitir la integración con:

* Sistemas internos del Instituto Finlay
* Estándares internacionales de farmacovigilancia (ej. HL7, FHIR)

---

### RF-10 Soporte multiplataforma

El sistema deberá estar disponible como:

* Aplicación web
* Aplicación móvil (Android / iOS)

---

# ⚙️ Requerimientos No Funcionales (RNF)

## 🔐 Seguridad y privacidad

### RNF-01

El sistema deberá cumplir con principios de **protección de datos personales**, asegurando:

* Anonimización de datos sensibles
* Cifrado en tránsito y en reposo

### RNF-02

El sistema deberá registrar auditorías de acceso y modificaciones.

---

## 🚀 Rendimiento

### RNF-03

El sistema deberá soportar **al menos N usuarios concurrentes** sin degradación perceptible del servicio.

### RNF-04

El tiempo de respuesta no deberá exceder los **2 segundos** para operaciones comunes.

---

## 🧩 Escalabilidad

### RNF-05

El sistema deberá ser escalable horizontalmente para manejar picos de reportes durante campañas de vacunación.

---

## 🧑‍💻 Usabilidad

### RNF-06

La interfaz deberá ser:

* Intuitiva
* Accesible
* Compatible con estándares WCAG

---

## 🛠️ Mantenibilidad

### RNF-07

El sistema deberá seguir una **arquitectura modular** que facilite:

* Actualizaciones
* Incorporación de nuevas vacunas o formularios

---

## 🔄 Disponibilidad

### RNF-08

El sistema deberá tener una disponibilidad mínima del **99%**.

---

## 📦 Portabilidad

### RNF-09

El sistema deberá poder desplegarse en:

* Infraestructura local
* Nube privada o híbrida

---

## 📊 Calidad de datos

### RNF-10

El sistema deberá validar datos para:

* Evitar inconsistencias
* Reducir reportes duplicados

---

## 🧠 (Plus para lucirte) Requerimientos de futuro

### RNF-11

El sistema deberá permitir la incorporación futura de:

* Modelos de ML para detección temprana de señales adversas
* Análisis predictivo de riesgos

---
