/// <summary>
/// Punto de entrada de la aplicación ASP.NET Core. Aquí se configura el host, los servicios
/// y el pipeline HTTP (Swagger, DbContext, repositorios y mappers).
/// Este archivo contiene declaraciones de nivel superior y no define clases públicas.
/// </summary>

using CunDropShipping.adapter.restful.v1.controller.Mapper;
using CunDropShipping.infrastructure.DbContext;
using CunDropShipping.application.Service;
using CunDropShipping.domain;
using CunDropShipping.infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Lee la cadena de conexion del archivo appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registra el AppDbContext en el contenedor de dependencias.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<Repository>();
builder.Services.AddScoped<IInfrastructureMapper, InfrastructureMapperImpl>();
builder.Services.AddScoped<IAdapterMapper, AdapterMapper>();
builder.Services.AddScoped<IProductService, ProductServiceImp>();

var app = builder.Build();

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
