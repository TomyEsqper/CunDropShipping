# Catalog Service

API backend construida con ASP.NET Core para gestionar un catalogo de productos y subcategorias.

## Resumen

Este repositorio contiene un servicio organizado en cuatro capas:

- `domain`: entidades de negocio y logica de servicios.
- `application`: contratos de servicio.
- `infrastructure`: persistencia con Entity Framework Core y mapeadores de infraestructura.
- `adapter`: controladores REST, DTOs de la API y mapeadores del adaptador.

La API actual expone:

- CRUD de productos
- busqueda de productos por nombre
- filtros de productos por precio y stock
- CRUD de subcategorias

## Estructura General

- `catalog-service/`: proyecto principal de la aplicacion.
- `catalog-service/domain/`: entidades del dominio e implementaciones de servicios.
- `catalog-service/application/Service/`: interfaces de servicios de aplicacion.
- `catalog-service/infrastructure/`: repositorios EF Core, entidades de persistencia y mapeadores.
- `catalog-service/adapter/restful/v1/controller/`: controladores, modelos expuestos por la API y mapeadores.
- `docs/`: documentacion detallada del proyecto.
- `catalog-service.Tests/`: integration tests de la API.

Documentacion adicional:

- [Arquitectura](docs/architecture.md)
- [Estructura del proyecto](docs/project-structure.md)
- [Resumen de la API](docs/api-overview.md)
- [Contribucion](docs/contributing.md)

## Requisitos

- .NET SDK en la version definida en `global.json`
- acceso a una base de datos compatible con MySQL

## Ejecucion Local

Restaurar dependencias:

```bash
dotnet restore catalog-service.sln
```

Compilar la solucion:

```bash
dotnet build catalog-service.sln
```

Ejecutar la API:

```bash
dotnet run --project catalog-service/catalog-service.csproj
```

Ejecutar con hot reload:

```bash
dotnet watch --project catalog-service/catalog-service.csproj run
```

Ejecutar tests de integracion:

```bash
dotnet test catalog-service.sln
```

Los perfiles locales de ejecucion estan definidos en `catalog-service/Properties/launchSettings.json`.

## Configuracion

El servicio lee la conexion a base de datos desde `ConnectionStrings:DefaultConnection`.

Para desarrollo local, es mejor sobrescribirla con una variable de entorno en lugar de editar configuracion versionada:

```bash
ConnectionStrings__DefaultConnection=Server=localhost;Port=3306;Database=catalog;User=app;Password=your-password;SslMode=None;
```

Revisa [`catalog-service/appsettings.json`](catalog-service/appsettings.json) con cuidado antes de confirmar cambios de configuracion.

## Swagger

Swagger solo se habilita en el entorno `Development`. Con los perfiles locales actuales, la aplicacion abre Swagger automaticamente.

## Notas Actuales

- La serializacion JSON devuelve los enums como texto.
- `ProductServiceImp` valida que la subcategoria referenciada exista antes de crear o actualizar un producto.
- Las consultas de productos incluyen la subcategoria relacionada en las lecturas del repositorio.

## Flujo de Trabajo

- modelo de ramas: estilo Git Flow (`main`, `Develop`, `feature/*`, `docs/*`)
- estilo de commits: Conventional Commits
- los cambios de documentacion deberian ir separados de cambios funcionales cuando sea posible
