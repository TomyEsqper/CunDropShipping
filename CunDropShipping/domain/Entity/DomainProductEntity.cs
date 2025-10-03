namespace CunDropShipping.domain.Entity;

/// <summary>
/// Entidad del dominio que representa un producto dentro de la lógica de negocio.
/// Contiene sólo los campos necesarios para las operaciones de la capa de negocio.
/// </summary>
public class DomainProductEntity
{
    /// <summary>
    /// Identificador único del producto en el dominio.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Descripción del producto.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Precio del producto en la moneda utilizada por la aplicación.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Cantidad disponible en stock.
    /// </summary>
    public int Stock { get; set; }
}