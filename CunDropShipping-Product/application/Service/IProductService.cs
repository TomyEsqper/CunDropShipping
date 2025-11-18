//Importamos la entidad de dominio para poder usarla en nuestro contrato
using CunDropShipping.domain.Entity;

namespace CunDropShipping.application.Service;

/// <summary>
/// Contrato del servicio de productos que expone operaciones CRUD y consultas de filtrado.
/// Las implementaciones deben encargarse de la lógica de negocio relacionada con productos.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Obtiene todos los productos disponibles.
    /// </summary>
    /// <returns>Lista de <see cref="DomainProductEntity"/> que representan los productos.</returns>
    List<DomainProductEntity> GetAllProducts();

    /// <summary>
    /// Obtiene un producto por su identificador único.
    /// </summary>
    /// <param name="id">Identificador del producto a recuperar.</param>
    /// <returns>La entidad <see cref="DomainProductEntity"/> correspondiente al id o null si no existe.</returns>
    DomainProductEntity GetProductById(int id);

   /// <summary>
    /// Crea y persiste un nuevo producto.
    /// </summary>
    /// <param name="product">Entidad de dominio que contiene los datos del producto a guardar.</param>
    /// <returns>La entidad <see cref="DomainProductEntity"/> creada (normalmente con Id generado).</returns>
    DomainProductEntity SaveProduct(DomainProductEntity product);

    /// <summary>
    /// Actualiza un producto existente identificado por su id.
    /// </summary>
    /// <param name="id">Identificador del producto a actualizar.</param>
    /// <param name="product">Entidad de dominio con los valores a actualizar.</param>
    /// <returns>La entidad <see cref="DomainProductEntity"/> actualizada o null si no existe el producto.</returns>
    DomainProductEntity UpdateProduct(int id, DomainProductEntity product);

    /// <summary>
    /// Elimina un producto existente.
    /// </summary>
    /// <param name="idProduct"></param>
    /// <returns>La entidad <see cref="DomainProductEntity"/> eliminada o null si no se encontró.</returns>
    DomainProductEntity DeleteProduct(int idProduct);
    
    // --- Metodos filtrado inteligente ---
    /// <summary>
    /// Busca productos cuyo nombre contenga el término indicado.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda para filtrar por nombre.</param>
    /// <returns>Lista de <see cref="DomainProductEntity"/> que coinciden con la búsqueda.</returns>
    List<DomainProductEntity> SearchProductsByName(string searchTerm);

    /// <summary>
    /// Filtra productos cuyo precio esté dentro de un rango especificado.
    /// </summary>
    /// <param name="minPrice">Precio mínimo inclusivo.</param>
    /// <param name="maxPrice">Precio máximo inclusivo.</param>
    /// <returns>Lista de <see cref="DomainProductEntity"/> que cumplen el rango de precio.</returns>
    List<DomainProductEntity> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice);

    /// <summary>
    /// Obtiene los productos cuyo stock es menor o igual a un umbral.
    /// </summary>
    /// <param name="stockThreshold">Umbral de stock para considerar bajo stock.</param>
    /// <returns>Lista de <see cref="DomainProductEntity"/> con stock bajo.</returns>
    List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold);
}