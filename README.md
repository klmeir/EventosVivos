# Eventos Vivos - Sistema de Reservas

Núcleo del sistema de gestión de reservas de eventos, construido bajo principios de **Clean Architecture**, **CQRS** y **Domain-Driven Design (DDD)**.   

Desarrollado con **.NET 10** y **Angular 22**.

Incluye:
- Commands / Queries con MediatR
- Pipeline Behaviors (Logging, Validation)
- Manejo global de errores (RFC 7807)
- CorrelationId para trazabilidad
- Tests unitarios

---

## 🚀 Arquitectura del Sistema

La solución implementa una separación estricta de responsabilidades en capas concéntricas, garantizando que el dominio sea el núcleo agnóstico de toda la tecnología de infraestructura.

```text
/src                            # Código fuente del Backend (.NET)
  /EventosVivos.Domain          # Entidades, Value Objects
  /EventosVivos.Application     # CQRS (MediatR), Pipeline Behaviors, Validaciones, DTOs
  /EventosVivos.Infrastructure  # EF Core, Repositorios, SQLite
  /EventosVivos.Api             # Endpoints de API, Middlewares (Error, CorrelationId)

/eventos-vivos-app              # Código fuente del Frontend (Angular)
  /src/app                      # Componentes, servicios, modelos

/tests
  /EventosVivos.Domain.Tests    # Pruebas de Dominio y Value Objects  
```

## 🔒 Manejo de Concurrencia

Para garantizar la integridad en la reserva de cupos y evitar la sobreventa de entradas, el sistema implementa una estrategia de **Concurrencia Optimista**:

* **Implementación:** Se utiliza un *Concurrency Token* manual en las entidades críticas.
* **Funcionamiento:** Cada vez que una reserva es procesada, el sistema verifica que la versión del registro no haya cambiado desde el momento en que se leyó la disponibilidad del evento.
* **Manejo de Conflictos:** En caso de que dos usuarios intenten reservar el último cupo simultáneamente:
    1. La primera transacción se completa exitosamente.
    2. La segunda transacción detecta un conflicto de versión al intentar persistir los cambios.
    3. El sistema captura la `DbUpdateConcurrencyException`, la traduce mediante el `ExceptionHandlingMiddleware` y devuelve una respuesta estructurada con estado 409 Conflict y el siguiente mensaje:
      "This record was updated by another user. Please try again."



---

## 🔑 Configuración de Autenticación (Admin)
El sistema utiliza autenticación basada en credenciales configuradas estáticamente. No se requiere registro de usuarios administradores en la base de datos.

Ruta de configuración: src/EventosVivos.Api/appsettings.json
```bash
{
  "AdminCredentials": {
    "Username": "admin",
    "Password": "Password123!"
  }
}
```

Notas Importantes:
Validación en Memoria: El sistema valida las credenciales de administrador directamente contra los valores definidos en el archivo appsettings.json.

---

🛠️ Prerrequisitos

Antes de empezar, asegúrate de tener instalado en tu máquina:

**.NET 10 SDK** y **Node 24.16+**.   
**Docker y Docker Compose**.

---

## ▶️ Instrucciones de Arranque y Ejecución

# 🐳 Docker y Docker Compose

```bash
docker-compose build --no-cache
docker-compose up -d
```


| Aplicación             | URL o Servicio                |
| ---------------------- | ----------------------------- |
| **API Backend**        | [http://localhost:8000](http://localhost:8000) |
| **Frontend Angular**   | [http://localhost:4200](http://localhost:4200) |

---

## 🧪 Ejecución de la Suite de Tests

```bash
dotnet test
```
---
