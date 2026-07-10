using System.Text.Json.Serialization;
using Catalog.adapter.restful.v1.controller.Mapper;
using Catalog.infrastructure.DbContext;
using Catalog.application.Service;
using Catalog.domain;
using Catalog.infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Esta línea obliga a C# a mostrar el texto del Enum en lugar de su número interno
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Lee la cadena de conexion del archivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registra el AppDbContext en el contenedor de dependencias.
if (builder.Environment.IsEnvironment("Testing"))
{
    var testDatabasePath = Path.Combine(Path.GetTempPath(), "catalog-service-integration-tests.db");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite($"Data Source={testDatabasePath}"));
}
else
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IProductInfrastructureMapper, ProductInfrastructureMapperImpl>();
builder.Services.AddScoped<IProductAdapterMapper, ProductAdapterMapper>();
builder.Services.AddScoped<IProductService, ProductServiceImp>();

builder.Services.AddScoped<SubCategoryRepository>();
builder.Services.AddScoped<ISubCategoryInfrastructureMapper, SubCategoryInfrastructureMapperImpl>();
builder.Services.AddScoped<ISubCategoryAdapterMapper, SubCategoryAdapterMapper>();
builder.Services.AddScoped<ISubCategoryService, SubCategoryServiceImp>();

builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ICategoryInfrastructureMapper, CategoryInfrastructureMapperImpl>();
builder.Services.AddScoped<ICategoryAdapterMapper, CategoryAdapterMapper>();
builder.Services.AddScoped<ICategoryService, CategoryServiceImp>();

var app = builder.Build();

if (app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
