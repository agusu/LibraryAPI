# Library API

Una API REST moderna para gestionar una biblioteca, construida con .NET 9 Preview, EF Core y MySQL.

## Características

- ✅ CRUD completo para Autores y Libros
- 🔐 Autenticación JWT
- 📚 Relaciones entre entidades
- 🧪 Tests unitarios
- 🐳 Dockerizado
- 📝 Documentación Swagger

## Tecnologías

- .NET 9 Preview
- Entity Framework Core
- MySQL
- AutoMapper
- MediatR
- JWT Authentication
- xUnit & Moq
- Docker

## Inicio Rápido

1. **Clonar y construir**

```bash
git clone https://github.com/tuusuario/library-api
cd library-api
docker-compose build
```

2. **Ejecutar**

```bash
docker-compose up -d
```

3. **Probar API**

- Swagger UI: http://localhost:8080/swagger
- Postman: Importar `Library API.postman_collection.json`

## Estructura del Proyecto

```
Library/
├── API/ # Controladores y configuración
├── Application/ # Lógica de negocio (CQRS)
├── Domain/ # Entidades y interfaces
├── Infrastructure/ # Implementaciones concretas
└── Tests/ # Tests unitarios
```

## Endpoints

### Auth

- POST `/api/auth/login` - Obtener token JWT

### Authors

- GET `/api/authors` - Listar autores
- GET `/api/authors/{id}` - Obtener autor
- POST `/api/authors` - Crear autor
- PUT `/api/authors/{id}` - Actualizar autor
- DELETE `/api/authors/{id}` - Eliminar autor

### Books

- GET `/api/books` - Listar libros
- GET `/api/books/{id}` - Obtener libro
- GET `/api/books/byauthor/{authorId}` - Obtener libros por autor
- POST `/api/books` - Crear libro
- PUT `/api/books/{id}` - Actualizar libro
- DELETE `/api/books/{id}` - Eliminar libro

## Tests

```bash
dotnet test
```

![Test ejecutados](https://i.imgur.com/QJdZQxg.png)

# Documentación de Arquitectura

## Decisiones Técnicas

### 1. Clean Architecture

- **Capas**:
  - `Domain`: Entidades y reglas de negocio core
  - `Application`: Casos de uso y lógica de aplicación
  - `Infrastructure`: Implementaciones concretas (DB, servicios externos)
  - `API`: Controllers y configuración
- **Beneficios**:
  - Independencia de frameworks
  - Testeable
  - Mantenible

### 2. CQRS con MediatR

- **Commands**: Modifican estado (Create, Update, Delete)
- **Queries**: Consultan datos (GetAll, GetById, GetByAuthor)
- **Beneficios**:
  - Separación de responsabilidades
  - Escalabilidad
  - Facilita testing

### 3. Base de Datos

- **MySQL** con Entity Framework Core
- **Code First** con migraciones
- **Relaciones**:
  ```csharp
  modelBuilder.Entity<Book>()
      .HasOne(b => b.Author)
      .WithMany(a => a.Books)
      .HasForeignKey(b => b.AuthorId);
  ```

### 4. Seguridad

- JWT para autenticación
- Passwords hasheados (BCrypt)
- Autorización basada en roles

### 5. Validación

- FluentValidation para DTOs
- Validación de modelo automática
- Manejo centralizado de errores

### 6. Testing

- xUnit para tests unitarios
- Moq para mocking
- Tests por capa (Commands/Queries)

## Patrones Implementados

1. **Repository Pattern** (implícito via DbContext)
2. **Mediator** (MediatR)
3. **CQRS**
4. **DTO**
5. **Unit of Work** (DbContext)

## Flujo de Datos

1. Request HTTP → Controller
2. Controller → Command/Query (MediatR)
3. Handler → Business Logic
4. Business Logic → Repository
5. Repository → Database
6. Response ← DTO ← Entity

## Decisiones Destacables

1. **Async/Await**

   - Operaciones DB asíncronas
   - Mejor escalabilidad

2. **AutoMapper**

   - Mapeo Entity ↔ DTO
   - Configuración centralizada

3. **Docker**

   - Multi-stage build
   - Optimizado para producción

4. **Swagger**
   - Documentación automática
   - Testing de endpoints
