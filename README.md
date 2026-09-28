# 🎓 Sistema de Gestión de Aula

Sistema web completo para la gestión de calificaciones escolares, desarrollado con **.NET 10**, **Blazor Server**, **Entity Framework Core** y **SQL Server**.

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4?logo=blazor&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
[![EF Core](https://img.shields.io/badge/EF%20Core-10.0-512BD4?logo=nuget&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

---

## 📖 Descripción

Aplicación web para gestionar las calificaciones de un aula escolar. Permite visualizar el estado académico de cada alumno (aprobado / en riesgo / desaprobado), consultar su libreta completa con los 4 parciales por materia, identificar los cursos desaprobados con su profesor asignado, y obtener un resumen estadístico por materia.

El proyecto fue construido aplicando **arquitectura en capas**, **separación de responsabilidades** y **buenas prácticas de desarrollo** con el ecosistema .NET moderno.

---

## 📸 Capturas de pantalla

### 🏠 Dashboard
<img width="1892" height="700" alt="image" src="https://github.com/user-attachments/assets/ce416d72-107c-4089-abe2-9842f6d06d05" />

*Panel principal con estadísticas generales del aula.*

### 👥 Lista de Alumnos
<img width="1851" height="872" alt="image" src="https://github.com/user-attachments/assets/175ca6b7-f42a-444c-a1c2-c19e9a519251" />

*Tabla con filtros por nombre, DNI y estado académico.*

### 📖 Libreta Individual
<img width="1555" height="897" alt="image" src="https://github.com/user-attachments/assets/0a73d276-c82e-45fb-8b77-0fa219cd1040" />

*Detalle de las 4 notas parciales por materia con promedio.*

### ⚠️ Cursos Desaprobados
<img width="1557" height="850" alt="image" src="https://github.com/user-attachments/assets/5a1005db-af76-4bc2-b6b8-4a2f0a6f03fc" />

*Reporte de materias jaladas por alumno con profesor asignado.*

### 📚 Resumen por Materia
<img width="1562" height="742" alt="image" src="https://github.com/user-attachments/assets/ccf12b18-d292-4508-a1f5-6984f63286b1" />

*Estadísticas por materia: aprobados, desaprobados y promedio.*

---

## ✨ Características

- 📊 **Dashboard interactivo** con estadísticas generales del aula
- 👥 **Listado de alumnos** con búsqueda en tiempo real y filtros por estado
- 📖 **Libreta individual** mostrando los 4 parciales y el promedio por materia
- ⚠️ **Reporte de cursos desaprobados** con profesor, grado y sección
- 📚 **Resumen estadístico por materia** (aprobados, desaprobados, % y promedio)
- 🎨 **Interfaz responsiva** con Bootstrap 5
- 🔍 **Búsqueda instantánea** sin recargar la página
- 🏷️ **Badges dinámicos** según el estado académico (verde/amarillo/rojo)

---

## 🏗️ Arquitectura del proyecto

El proyecto sigue un patrón de **arquitectura en capas**, separando responsabilidades en 4 proyectos:

```
GestionAula.sln
│
├── src/
│   ├── GestionAula.Shared/     → DTOs compartidos entre API y Blazor
│   ├── GestionAula.Data/       → Acceso a datos con EF Core
│   ├── GestionAula.Api/        → Web API REST (.NET 10)
│   └── GestionAula.Web/        → Interfaz de usuario (Blazor Server)
│
├── Auladata.sql                → Script de creación de BD y datos de prueba
├── README.md
└── .gitignore
```

### 🔄 Flujo de datos

```
┌─────────────────┐   HTTP    ┌─────────────────┐   EF Core   ┌─────────────────┐
│  Blazor Server  │ ────────► │   Web API REST  │ ──────────► │   SQL Server    │
│   (Frontend)    │           │   (Backend)     │             │   (Database)    │
└─────────────────┘           └─────────────────┘             └─────────────────┘
       │                              │
       │                              │
       ▼                              ▼
  GestionAula.Web               GestionAula.Api
  - Components/Pages            - Controllers/
  - Services/ApiService         - Program.cs
       ▲
       │
       │ consume
       │
  GestionAula.Shared
  - DTOs (compartidos)
```

---

## 🧩 Descripción de cada proyecto

### 📦 `GestionAula.Shared` — DTOs

Contiene los **Data Transfer Objects** que se usan para transportar datos entre el API y Blazor. Evita exponer las entidades de EF Core directamente al frontend.

| DTO | Propósito |
|-----|-----------|
| `AlumnoDto` | Datos de alumno con promedio general, materias aprobadas/desaprobadas y estado académico |
| `NotaDto` | Nota individual con sus 4 parciales, profesor y promedio por materia |
| `LibretaDto` | Libreta completa de un alumno (info + lista de notas) |
| `MateriaResumenDto` | Estadísticas de una materia (total, aprobados, desaprobados, %, promedio) |
| `AlumnoDesaprobadoDto` | Alumno con un curso jalado (materia, profesor, grado, sección) |

### 🗄️ `GestionAula.Data` — Acceso a datos

Capa de persistencia con **Entity Framework Core (Database First)**.

| Componente | Descripción |
|-----------|-------------|
| `Data/GestionAulaContext.cs` | DbContext con todos los DbSet y configuración de entidades |
| `Models/Alumno.cs` | Entidad generada por scaffold |
| `Models/Profesore.cs` | Entidad de profesor |
| `Models/Materia.cs` | Entidad de materia |
| `Models/Nota.cs` | Entidad de notas (4 parciales) |
| `Models/Matricula.cs` | Relación alumno ↔ grado/sección |
| `Models/ProfesorMateria.cs` | Relación N:M profesor ↔ materia |
| `Models/Grado.cs`, `Models/Seccione.cs` | Catálogos |

### 🌐 `GestionAula.Api` — Backend REST

API REST con **ASP.NET Core 10** usando Controllers. Expone los endpoints que consume Blazor.

| Controller | Endpoints |
|-----------|-----------|
| `AlumnosController` | `GET /api/alumnos`<br>`GET /api/alumnos/{id}/libreta`<br>`GET /api/alumnos/desaprobados` |
| `MateriasController` | `GET /api/materias/resumen`<br>`GET /api/materias/{id}/aprobados`<br>`GET /api/materias/{id}/desaprobados` |

**Características técnicas:**
- Inyección de dependencias del `DbContext`
- Programación asíncrona (`async/await`)
- `AsNoTracking()` para consultas de solo lectura
- Manejo de profesores múltiples por materia (agrupación con `GroupBy`)

### 🎨 `GestionAula.Web` — Frontend Blazor

Aplicación **Blazor Server** con interactividad en tiempo real vía SignalR.

| Componente | Descripción |
|-----------|-------------|
| `Components/Pages/Home.razor` | Dashboard con tarjetas de estadísticas |
| `Components/Pages/Alumnos.razor` | Tabla con filtros y búsqueda en vivo |
| `Components/Pages/Libreta.razor` | Libreta detallada de un alumno |
| `Components/Pages/Desaprobados.razor` | Reporte de cursos jalados |
| `Components/Pages/Materias.razor` | Tarjetas con resumen por materia |
| `Components/Layout/NavMenu.razor` | Menú lateral de navegación |
| `Components/Layout/MainLayout.razor` | Layout principal |
| `Services/ApiService.cs` | Cliente HTTP tipado hacia el API |

**Características técnicas:**
- `HttpClientFactory` para manejo eficiente de conexiones
- Servicio tipado (`ApiService`) con métodos por endpoint
- Renderizado condicional (loading / data / error)
- `@bind` bidireccional para filtros en tiempo real

---

## 🛠️ Tecnologías utilizadas

| Capa | Tecnología | Versión |
|------|-----------|---------|
| **Backend** | ASP.NET Core Web API | .NET 10 |
| **Frontend** | Blazor Server | .NET 10 |
| **ORM** | Entity Framework Core | 10.0.12 |
| **Base de datos** | SQL Server | 2022 |
| **UI Framework** | Bootstrap | 5 |
| **Pruebas API** | Postman | — |
| **IDE** | Visual Studio | 2026 |
| **Control de versiones** | Git + GitHub | — |

---

## 📊 Modelo de datos

El sistema maneja **8 tablas** con relaciones bien definidas:

```
<img width="645" height="771" alt="image" src="https://github.com/user-attachments/assets/e3e0393d-902f-458e-9ae2-ae7e3cfd6e5a" />
<img width="805" height="756" alt="image" src="https://github.com/user-attachments/assets/cfd412f3-c33a-47f9-84b7-e99582900826" />


```
<img width="805" height="756" alt="image" src="https://github.com/user-attachments/assets/a2e27229-aa9e-4084-ad7d-7f9c10868309" />


**Reglas de negocio:**
- Cada alumno pertenece a un grado y una sección (vía `Matriculas`)
- Cada alumno tiene exactamente 4 notas por materia (Parcial 1-4)
- El promedio se calcula como `(P1 + P2 + P3 + P4) / 4`
- Un alumno **aprueba** una materia si su promedio ≥ 11
- Un profesor puede enseñar **hasta 3 materias** (`ProfesorMateria`)
- Una materia puede tener **múltiples profesores**

**Datos de prueba incluidos:**
- 👥 30 alumnos
- 👨‍🏫 10 profesores
- 📚 10 materias
- 📝 300 notas (30 × 10 materias)
- 🏫 3 grados × 3 secciones

---

## 🚀 Cómo ejecutar el proyecto

### 📋 Requisitos previos

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server 2019+](https://www.microsoft.com/sql-server) (Express, Developer o superior)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) o [VS Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/)

### 🔧 Instalación paso a paso

#### 1️⃣ Clonar el repositorio

```bash
git clone https://github.com/davidhuaman-2008/GestionAula.git
cd GestionAula
```

#### 2️⃣ Crear la base de datos

Abre **SQL Server Management Studio** o **Azure Data Studio** y ejecuta el script:

```sql
-- Ejecutar el archivo Auladata.sql
```

Este script:
- Crea la BD `GestionAula`
- Crea las 8 tablas con sus constraints
- Inserta los datos de prueba (30 alumnos, 300 notas, etc.)

#### 3️⃣ Configurar la cadena de conexión

Edita el archivo `src/GestionAula.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "GestionAulaConnection": "Server=localhost;Database=GestionAula;User Id=TU_USUARIO;Password=TU_PASSWORD;TrustServerCertificate=True;Encrypt=True;"
  }
}
```

> 💡 **Tip**: si usas autenticación de Windows, reemplaza por:
> `"Server=localhost;Database=GestionAula;Trusted_Connection=True;TrustServerCertificate=True;"`

#### 4️⃣ Ejecutar la aplicación

**Terminal 1** — Ejecutar el API:

```bash
dotnet run --project src/GestionAula.Api
```

Verás:
```
Now listening on: http://localhost:5128
```

**Terminal 2** — Ejecutar Blazor:
<img width="1097" height="867" alt="image" src="https://github.com/user-attachments/assets/54d1a99f-5676-45a4-b6dc-9347733285cf" />

```bash
dotnet run --project src/GestionAula.Web
```

Verás:
```
Now listening on: http://localhost:5234
Now listening on: https://localhost:7136
```

#### 5️⃣ Abrir en el navegador

```
http://localhost:5234
```

---

## 📡 Documentación de la API

### 👥 Alumnos

#### `GET /api/alumnos`
Devuelve la lista completa de alumnos con promedio general y estado.

**Respuesta:**
```json
[
  {
    "alumnoId": 1,
    "nombres": "Juan",
    "apellidos": "Pérez López",
    "dni": "70000001",
    "email": "jperez@mail.com",
    "grado": "1ro Secundaria",
    "seccion": "A",
    "anioEscolar": 2025,
    "promedioGeneral": 10.08,
    "materiasAprobadas": 2,
    "materiasDesaprobadas": 8,
    "estadoGeneral": "DESAPROBADO"
  }
]
```

#### `GET /api/alumnos/{id}/libreta`
Devuelve la libreta completa de un alumno con sus 4 parciales por materia.

#### `GET /api/alumnos/desaprobados`
Lista todos los cursos desaprobados con alumno, materia y profesor.

### 📚 Materias

#### `GET /api/materias/resumen`
Resumen estadístico por materia (total, aprobados, desaprobados, % y promedio).

#### `GET /api/materias/{id}/aprobados`
Alumnos aprobados en una materia específica.

#### `GET /api/materias/{id}/desaprobados`
Alumnos desaprobados en una materia específica.

> 📮 **Colección de Postman**: importa el archivo `GestionAula-API.postman_collection.json` para probar todos los endpoints fácilmente.

---

## 🎯 Reglas de negocio implementadas

| Regla | Implementación |
|-------|---------------|
| Nota aprobatoria = 11 | `promedio >= 11 ? "APROBADO" : "DESAPROBADO"` |
| Estado "EN RIESGO" | Alumno con 1-2 materias desaprobadas |
| Estado "DESAPROBADO" | Alumno con 3+ materias desaprobadas |
| Promedio por materia | `(P1 + P2 + P3 + P4) / 4` |
| Promedio general | Media de todos los promedios por materia |
| Rango de notas | 0 a 20 (constraint en BD) |

---

## 📁 Estructura de carpetas detallada

```
GestionAula/
│
├── src/
│   ├── GestionAula.Api/
│   │   ├── Controllers/
│   │   │   ├── AlumnosController.cs
│   │   │   └── MateriasController.cs
│   │   ├── Properties/
│   │   │   └── launchSettings.json
│   │   ├── appsettings.json
│   │   ├── appsettings.Development.json
│   │   └── Program.cs
│   │
│   ├── GestionAula.Data/
│   │   ├── Data/
│   │   │   └── GestionAulaContext.cs
│   │   └── Models/
│   │       ├── Alumno.cs
│   │       ├── Grado.cs
│   │       ├── Materia.cs
│   │       ├── Matricula.cs
│   │       ├── Nota.cs
│   │       ├── Profesore.cs
│   │       ├── ProfesorMaterium.cs
│   │       └── Seccione.cs
│   │
│   ├── GestionAula.Shared/
│   │   └── DTOs/
│   │       ├── AlumnoDto.cs
│   │       ├── AlumnoDesaprobadoDto.cs
│   │       ├── LibretaDto.cs
│   │       ├── MateriaResumenDto.cs
│   │       └── NotaDto.cs
│   │
│   └── GestionAula.Web/
│       ├── Components/
│       │   ├── Layout/
│       │   │   ├── MainLayout.razor
│       │   │   └── NavMenu.razor
│       │   ├── Pages/
│       │   │   ├── Home.razor
│       │   │   ├── Alumnos.razor
│       │   │   ├── Libreta.razor
│       │   │   ├── Desaprobados.razor
│       │   │   └── Materias.razor
│       │   ├── App.razor
│       │   ├── Routes.razor
│       │   └── _Imports.razor
│       ├── Services/
│       │   └── ApiService.cs
│       ├── wwwroot/
│       ├── appsettings.json
│       └── Program.cs
│
├── docs/
│   └── screenshots/
│       ├── dashboard.png
│       ├── alumnos.png
│       ├── libreta.png
│       ├── desaprobados.png
│       └── materias.png
│
├── Auladata.sql
├── GestionAula.slnx
├── README.md
├── LICENSE
└── .gitignore
```

---

## 🧠 Conceptos aplicados

- ✅ **Arquitectura en capas** (Shared / Data / Api / Web)
- ✅ **DTOs** para desacoplar frontend de base de datos
- ✅ **Inyección de dependencias** (DbContext, HttpClientFactory, servicios)
- ✅ **Entity Framework Core Database First** (scaffold)
- ✅ **Programación asíncrona** (async/await)
- ✅ **API REST** con Controllers y buenas prácticas HTTP
- ✅ **Blazor Server** con interactividad en tiempo real (SignalR)
- ✅ **Componentes reutilizables** (.razor)
- ✅ **Client-side filtering** para búsquedas en vivo
- ✅ **Manejo de errores** con try/catch y estados de carga
- ✅ **SOLID** (responsabilidad única por capa)
- ✅ **Query splitting** para consultas con múltiples Includes

---

## 🔮 Mejoras futuras

- [ ] Autenticación y autorización con JWT (roles: Admin, Profesor, Alumno)
- [ ] CRUD completo de alumnos, profesores y materias
- [ ] Exportación de libretas a PDF
- [ ] Gráficos con Chart.js (evolución de notas por parcial)
- [ ] Testing unitario con xUnit + Moq
- [ ] Dockerización con docker-compose
- [ ] CI/CD con GitHub Actions
- [ ] Deploy a Azure App Service

---

## 👨‍💻 Autor

**David Huaman**

- 🐙 GitHub: [@davidhuaman-2008](https://github.com/davidhuaman-2008)
- 📧 Email: dhuaman.2008@gmail.com

---

## 📄 Licencia

Este proyecto está bajo la Licencia MIT — ver el archivo [LICENSE](LICENSE) para más detalles.

---

## 🙏 Agradecimientos

Proyecto desarrollado como práctica personal para demostrar el uso de **.NET 10**, **Blazor Server**, **Entity Framework Core** y **SQL Server** aplicando arquitectura en capas y buenas prácticas.

---

<div align="center">

**⭐ Si te gustó este proyecto, dale una estrella en GitHub ⭐**

Hecho con ❤️ por [David Huaman](https://github.com/davidhuaman-2008)

</div>
