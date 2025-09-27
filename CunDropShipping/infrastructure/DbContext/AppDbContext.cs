using CunDropShipping.infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
namespace CunDropShipping.infrastructure.DbContext;

// Hereda de DbContext, la clase de Entity Framework Core.
public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    // Este es el constructor. Recibe las opciones de configuracion (como la cadena
    // y las pasa a la clase de EF Core.
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    // Esta linea crea una representacion de tu tabla 'Products' en el codigo.
    // A traves de 'Products', podremos hacer consultas como buscar, crear, eliminar, etc.
    public DbSet<ProductEntity> Products { get; set; }
}   