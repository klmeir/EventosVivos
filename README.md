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
  /EventosVivos.Domain          # Entidades, Value Objects, Memento, Especificaciones
  /EventosVivos.Application     # CQRS (MediatR), Pipeline Behaviors, Validaciones, DTOs
  /EventosVivos.Infrastructure  # EF Core, Repositorios, SQLite
  /EventosVivos.Api             # Endpoints de API, Middlewares (Error, CorrelationId)

/eventos-vivos-app              # Código fuente del Frontend (Angular)
  /src/app                      # Componentes, servicios, modelos

/tests
  /EventosVivos.Domain.Tests    # Pruebas de Dominio y Value Objects  
```

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
