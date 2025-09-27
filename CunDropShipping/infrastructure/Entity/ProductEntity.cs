using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CunDropShipping.infrastructure.Entity;

[Table("Productos_Tomas")]
public class ProductEntity
{
    // Estas columnas representan las columnas de la tabla 'Products'.
    // Los nombres y tipos de datos deben coincidir con la estructura de la tabla.
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    
    [StringLength(500)]
    public string Description { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public int Stock { get; set; }
}