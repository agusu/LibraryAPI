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
- POST `/api/books` - Crear libro
- PUT `/api/books/{id}` - Actualizar libro
- DELETE `/api/books/{id}` - Eliminar libro

## Tests

```bash
dotnet test
```
