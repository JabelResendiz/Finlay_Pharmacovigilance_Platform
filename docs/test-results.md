# 📊 Resultados de Pruebas - ESAVI Report

> **Última actualización:** 01/09/2026  
> **Responsable:** Jabel Resendiz Aguirre  
> **Versión del sistema:** v1.0.0

---

## 🧪 Resumen General

| **Tipo de Prueba** | **Estado** | **Última Ejecución** | **Resultado** |
| :--- | :--- | :--- | :--- |
| Unitarias (Backend) | ✅ Pasó | 01/09/2026 | 92% cobertura |
| Unitarias (Frontend) | ✅ Pasó | 01/09/2026 | 68% cobertura |
| Integración (Backend) | ✅ Pasó | 01/09/2026 | 85% cobertura |
| Carga (k6) | ✅ Pasó | 30/08/2026 | Ver detalles abajo |
| Usabilidad (SUS) | ✅ Pasó | 25/08/2026 | 90/100 - Excelente |
| Accesibilidad (WCAG) | ⚠️ Parcial | 20/08/2026 | 89-98 puntos |

---

## 🚀 Pruebas de Carga (k6)

### Escenario 1: Creación de Reportes

| **Usuarios Concurrentes (VUs)** | **Latencia p95 (ms)** | **Throughput (req/s)** | **Tasa de Éxito** |
| :--- | :--- | :--- | :--- |
| 5 | 172.8 | 5.2 | 100% |
| 10 | 315.8 | 10.1 | 100% |
| 30 | 623.7 | 25.3 | 100% |
| 50 | 1,158.1 | 24.8 | 100% |
| 70 | 2,147.0 | 23.1 | 100% |
| **100** | **4,033.2** | **22.0** | **74.8%** ⚠️ |
| 150 | 6,706.2 | 18.5 | 19.1% ❌ |
| 200 | 8,005.9 | 15.2 | 10.7% ❌ |

**Conclusión:** El sistema soporta hasta **70 usuarios concurrentes** sin degradación significativa. A partir de 100 usuarios, la latencia supera los 3 segundos y la tasa de éxito comienza a caer.

---

### Escenario 2: Búsqueda por Número de Notificación

| **Volumen de Datos (reportes)** | **Usuarios Concurrentes (VUs)** | **Latencia p95 (ms)** |
| :--- | :--- | :--- |
| 1,000 | 50 | 553.8 |
| 4,000 | 50 | 624.6 |
| 10,000 | 50 | 723.9 |
| 50,000 | 50 | 780.4 |
| 100,000 | 50 | 950.6 |

**Conclusión:** La búsqueda por notificación escala bien hasta 100,000 reportes gracias al índice B-Tree en la base de datos.

---

### Escenario 3: Escenario Combinado (100 VUs)

| **Operación** | **Solicitudes** | **Mediana (ms)** | **p95 (ms)** |
| :--- | :--- | :--- | :--- |
| Crear reporte | 1,078 | 858.3 | 2,468.4 |
| Buscar reporte | 581 | 236.0 | 551.5 |
| Asignar | 84 | 433.2 | 1,805.3 |
| Evaluar | 68 | 382.2 | 911.9 |
| Dashboard | 38 | 368.1 | 1,209.3 |

**Conclusión:** El escenario combinado simula una carga realista con 75 ciudadanos, 10 jefes municipales y 15 médicos. El sistema responde con una latencia global p95 de **1,623 ms** y una tasa de éxito del **99.5%**.

---

## 🧑‍💻 Pruebas de Usabilidad (SUS)

| **Participante** | **Perfil** | **Puntaje SUS** | **Categoría** |
| :--- | :--- | :--- | :--- |
| P1 | Ciudadano | 92.5 | Excelente |
| P2 | Ciudadano | 95.0 | Excelente |
| P3 | Ciudadano | 80.0 | Excelente |
| P4 | Ciudadano | 95.0 | Excelente |
| P5 | Ciudadano | 92.5 | Excelente |
| A1 | Administrador | 85.0 | Excelente |
| A2 | Administrador | 90.0 | Excelente |
| **Promedio** | - | **90.0** | **Excelente** |

**Conclusión:** Los usuarios califican el sistema como "Excelente" en usabilidad. La facilidad de aprendizaje (4.9/5) es el aspecto mejor valorado.

> **Pendiente:** Realizar pruebas con médicos y jefes municipales (mínimo 3 por perfil).

---

## ♿ Pruebas de Accesibilidad (WCAG 2.1)

| **Página** | **Escritorio** | **Móvil** |
| :--- | :--- | :--- |
| Página principal | 98 | 93 |
| Registro de ESAVI | 89 | 89 |
| Panel informativo | 98 | 95 |

**Incidencias identificadas:**
- Botones sin nombre accesible (`aria-label` faltante).
- Barras de progreso sin etiqueta.
- Jerarquía de encabezados incorrecta.

**Estado:** Corregidas en la versión v1.0.1.

---

## 📈 Evolución de Resultados

| **Fecha** | **Versión** | **Cambio** | **Impacto** |
| :--- | :--- | :--- | :--- |
| 15/08/2026 | v1.0.0 | Primera prueba de carga | Límite: 70 VUs |
| 25/08/2026 | v1.0.1 | Mejora de índices en BD | Búsqueda un 20% más rápida |
| 30/08/2026 | v1.0.2 | Implementación de caché (Redis) | Pendiente de prueba |

---

## 🛠️ Cómo ejecutar las pruebas

### Pruebas de carga (k6)
```bash
# Instalar k6
brew install k6  # macOS
sudo apt install k6  # Linux

# Ejecutar prueba de creación de reportes
k6 run tests/k6-scripts/create-report.js

# Ejecutar prueba combinada
k6 run tests/k6-scripts/comprehensive-scenario.js