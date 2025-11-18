using CunDropShipping.infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace CunDropShipping.infrastructure.DbContext;

// Hereda de DbContext, la clase de Entity Framework Core.
/// <summary>
/// Contexto de la aplicación que representa la conexión a la base de datos y las colecciones (DbSet) disponibles.
/// Se configura en <c>Program.cs</c> y es utilizado por los repositorios para acceder a las tablas.
/// </summary>
public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="AppDbContext"/> con las opciones proporcionadas.
    /// </summary>
    /// <param name="options">Opciones de configuración para el DbContext (cadena de conexión, proveedor, etc.).</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    /// <summary>
    /// Representa la tabla de productos en la base de datos.
    /// A través de esta propiedad se pueden realizar consultas y cambios sobre la tabla "Productos_Tomas".
    /// </summary>
    public DbSet<ProductEntity> Products { get; set; }
}
