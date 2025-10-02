namespace CunDropShipping.adapter.restful.v1.controller.Entity;

/// <summary>
/// Entidad usada por el adaptador (API) que representa un producto en las respuestas y peticiones HTTP.
/// Esta clase corresponde al contrato público expuesto por la API REST.
/// </summary>
public class AdapterProductEntity
{
    /// <summary>
    /// Identificador único del producto expuesto por la API.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del producto que se muestra al cliente de la API.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Descripción del producto.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Precio del producto.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Cantidad de unidades disponibles en stock.
    /// </summary>
    public int Stock { get; set; }
}