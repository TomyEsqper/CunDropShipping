🚀 CunDropShipping API
======================

Introducción
------------
CunDropShipping API es un servicio backend construido en ASP.NET Core que implementa un catálogo de productos para una tienda de e-commerce. Este repositorio está diseñado como referencia práctica de buenas prácticas arquitectónicas (Clean Architecture), patrones de diseño y separación de responsabilidades.

Resumen rápido del contenido del repositorio
- Aplicación principal: `CunDropShipping/` (contiene `Program.cs`, `appsettings.json`, controladores, capas y migraciones).
- Migraciones EF Core: `CunDropShipping/Migrations/`.
- Controlador principal de la API: `adapter/restful/v1/controller/ProductController.cs`.

Arquitectura (4 capas)
----------------------
El proyecto sigue una Arquitectura Limpia de 4 capas:
- domain: entidades y lógica de negocio (independiente de frameworks).
- application: contratos (interfaces) y servicios de aplicación (por ejemplo `IProductService`).
- infrastructure: persistencia con EF Core (`AppDbContext`, `Repository`) y mappers de infraestructura.
- adapter: controladores REST y DTOs para entrada/salida (mappers entre DTOs y dominio).

Puntos relevantes de la implementación actual
- `Program.cs` registra `AppDbContext` usando MySQL (ServerVersion.AutoDetect) y registra via DI:
  - `Repository` (scoped)
  - `IInfrastructureMapper` → `InfrastructureMapperImpl`
  - `IAdapterMapper` → `AdapterMapper`
  - `IProductService` → `ProductServiceImp`
- `appsettings.json` contiene la sección `ConnectionStrings:DefaultConnection`. Recomendación: no dejar credenciales en el control de versiones; usar variables de entorno en entornos públicos.
- Las migraciones EF Core están incluidas en `Migrations/`.

Características principales
--------------------------
- CRUD de productos.
- Búsqueda por nombre, filtrado por rango de precio y consulta de productos con bajo stock.
- Mappers separados para mantener fronteras entre capas.

Endpoints expuestos (controlador `ProductController`)
-----------------------------------------------------
Base: `/api/v1/Products`

- GET /api/v1/Products
  - Descripción: Devuelve todos los productos.
  - Respuesta: 200 OK con lista de productos (formato `AdapterProductEntity`).

- GET /api/v1/Products/{id}
  - Descripción: Obtiene un producto por su id.
  - Respuesta: 200 OK con el producto o 404 Not Found si no existe.

- POST /api/v1/Products
  - Descripción: Crea un producto. Envía el cuerpo como JSON con el contrato público (`AdapterProductEntity`).
  - Respuesta: 201 Created con Location apuntando a `GET /api/v1/Products/{id}`.

- PUT /api/v1/Products/{id}
  - Descripción: Actualiza un producto existente (reemplazo parcial según implementación de mappers).
  - Respuesta: 200 OK con el producto actualizado o 404 Not Found si no existe.

- DELETE /api/v1/Products/{id}
  - Descripción: Elimina (según política implementada, puede tratarse como soft-delete) un producto.
  - Cuerpo: espera un `AdapterProductEntity` en el body (según controlador).
  - Respuesta: 200 OK con la entidad eliminada o 404 Not Found si no existe.

- GET /api/v1/Products/search?searchTerm={texto}
  - Descripción: Busca productos por nombre parcial o completo.
  - Parámetros: `searchTerm` (string).

- GET /api/v1/Products/filter/price?minPrice={min}&maxPrice={max}
  - Descripción: Filtra productos por rango de precio.
  - Parámetros: `minPrice` (decimal), `maxPrice` (decimal).

- GET /api/v1/Products/stock/low?stockThreshold={n}
  - Descripción: Devuelve productos con stock menor o igual a `stockThreshold`.
  - Parámetros: `stockThreshold` (int).

Modelos públicos (contrato expuesto por la API)
----------------------------------------------
`AdapterProductEntity` (campos usados por la API):
- Id (int)
- Name (string)
- Description (string)
- Price (decimal)
- Stock (int)

Cómo ejecutar localmente (pasos verificados)
--------------------------------------------
Checklist previo
- .NET SDK (usa la versión en `global.json` si está presente).
- MySQL o equivalente disponible.
- (Opcional) `dotnet-ef` para ejecutar migraciones si no estás usando migraciones automatizadas.

Pasos
1) Clonar el repositorio:

```bash
git clone https://github.com/TomyEsqper/CunDropShipping.git
cd CunDropShipping
```

2) Revisar la cadena de conexión en `CunDropShipping/appsettings.json` o sobrescribirla con una variable de entorno.
- Ejemplo: exportar la cadena sin credenciales en el archivo:

```bash
export ConnectionStrings__DefaultConnection="Server=localhost;Port=3306;Database=cundropshipping;User=root;Password=TuPassword;"
```

3) Aplicar migraciones (desde la carpeta `CunDropShipping`):

```bash
cd CunDropShipping
# Si no tienes dotnet-ef instalado globalmente:
# dotnet tool install --global dotnet-ef
dotnet ef database update
```

4) Ejecutar la aplicación:

```bash
# Desde la carpeta raíz del repositorio
dotnet run --project CunDropShipping
```

- Nota: `Program.cs` habilita Swagger sólo si el entorno es `Development`. Revisa la salida del comando para ver las URLs exactas (ej. `http://localhost:5000` y `https://localhost:5001`).

Ejemplos de uso prácticos
------------------------
- Listar productos:

```bash
curl -s "http://localhost:5000/api/v1/Products" | jq '.'
```

- Buscar por nombre:

```bash
curl -s "http://localhost:5000/api/v1/Products/search?searchTerm=camisa" | jq '.'
```

- Filtrar por precio:

```bash
curl -s "http://localhost:5000/api/v1/Products/filter/price?minPrice=10&maxPrice=100" | jq '.'
```

- Crear producto (JSON body):

```bash
curl -X POST "http://localhost:5000/api/v1/Products" \
  -H "Content-Type: application/json" \
  -d '{
        "name":"Camiseta ejemplo",
        "description":"Demo",
        "price":19.99,
        "stock":50
      }'
```

Flujo de trabajo y colaboración
-------------------------------
- Branching: Git Flow (main, develop, feature/*).
- Commits: Conventional Commits (`feat`, `fix`, `docs`, etc.).
- Pull Requests: incluir descripción, testing y revisar cambios en migraciones.