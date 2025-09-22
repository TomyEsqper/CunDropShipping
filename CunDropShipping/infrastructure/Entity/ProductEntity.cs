namespace CunDropShipping.infrastructure.Entity;

public class ProductEntity
{
    // Estas columnas representan las columnas de la tabla 'Products'.
    // Los nombres y tipos de datos deben coincidir con la estructura de la tabla.
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
}