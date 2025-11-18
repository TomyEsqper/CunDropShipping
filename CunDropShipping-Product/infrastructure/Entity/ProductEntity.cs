using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CunDropShipping.infrastructure.Entity;

[Table("products")]
/// <summary>
/// Representa la entidad de infraestructura que mapea la tabla de base de datos "Productos_Tomas".
/// Contiene las propiedades que corresponden a las columnas de la tabla.
/// </summary>
public class ProductEntity
{
    /// <summary>
    /// Identificador primario de la entidad en la base de datos.
    /// </summary>
    [Key]
    public int IdProduct { get; set; }
    
    /// <summary>
    /// Nombre del producto. Campo requerido y con longitud máxima de 100 caracteres.
    /// </summary>
    [Required]
    [StringLength(100)]
    public string nameProduct { get; set; }
    
    /// <summary>
    /// Descripción opcional del producto. Longitud máxima de 500 caracteres.
    /// </summary>
    [StringLength(500)]
    public string Description { get; set; }
    
    /// <summary>
    /// Precio del producto, mapeado en la base de datos como decimal(18,2).
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal price { get; set; }

    /// <summary>
    /// Cantidad disponible en stock.
    /// </summary>
    public int stockQuantity { get; set; }
}