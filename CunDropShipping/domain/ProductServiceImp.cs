// Añadimos los 'using' necesarios para el contrato y el repositorio
using CunDropShipping.application.Service;
using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.DbContext;
using CunDropShipping.infrastructure.Mapper;

namespace CunDropShipping.domain;

/// <summary>
/// Implementación del servicio de productos que delega las operaciones de persistencia al <see cref="Repository"/>.
/// Contiene la lógica de coordinación entre la capa de aplicación y la infraestructura.
/// </summary>
public class ProductServiceImp : IProductService
{
    // 1. EL ESPECIALISTA
    // Ya no tenemos una lista de juguete. Ahora tenemos una referencia
    // a nuestro "especialista en logistica", el IRepository.
    private readonly Repository _repository;

    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ProductServiceImp"/>.
    /// </summary>
    /// <param name="set">Instancia del repositorio que proporciona acceso a la persistencia.</param>
    public ProductServiceImp(Repository set)
    {
        _repository = set;
    }

    /// <summary>
    /// Obtiene todos los productos.
    /// </summary>
    /// <returns>Lista de <see cref="DomainProductEntity"/> representando los productos disponibles.</returns>
    public List<DomainProductEntity> GetAllProducts()
    {
        return _repository.GetAllProducts();
    }

    /// <summary>
    /// Obtiene un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto a recuperar.</param>
    /// <returns>La entidad <see cref="DomainProductEntity"/> correspondiente o propaga la excepción del repositorio si no existe.</returns>
    public DomainProductEntity GetProductById(int id)
    {
        return _repository.GetProductById(id);
    }

    /// <summary>
    /// Crea y persiste un nuevo producto.
    /// </summary>
    /// <param name="product">Entidad de dominio con los datos del producto a guardar.</param>
    /// <returns>La entidad creada con el Id generado por la base de datos.</returns>
    public DomainProductEntity SaveProduct(DomainProductEntity product)
    {
        return _repository.SaveProduct(product);
    }

    /// <summary>
    /// Actualiza un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto a actualizar.</param>
    /// <param name="product">Entidad de dominio con los nuevos valores.</param>
    /// <returns>La entidad actualizada o null si no existe el producto.</returns>
    public DomainProductEntity UpdateProduct(int id, DomainProductEntity product)
    {
        return _repository.UpdateProduct(id, product);
    }

    /// <summary>
    /// Elimina un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto a eliminar.</param>
    /// <param name="product">Entidad de dominio usada para validaciones opcionales.</param>
    /// <returns>La entidad eliminada o null si no existe.</returns>
    public DomainProductEntity DeleteProduct(int id, DomainProductEntity product)
    {
        return _repository.DeleteProduct(id, product);
    }

    /// <summary>
    /// Busca productos por nombre.
    /// </summary>
    /// <param name="searchTerm">Término utilizado para filtrar los nombres de producto.</param>
    /// <returns>Lista de productos que coinciden con el término.</returns>
    public List<DomainProductEntity> SearchProductsByName(string searchTerm)
    {
        return _repository.SearchProductsByName(searchTerm);
    }

    /// <summary>
    /// Filtra productos por rango de precio.
    /// </summary>
    /// <param name="minPrice">Precio mínimo inclusivo.</param>
    /// <param name="maxPrice">Precio máximo inclusivo.</param>
    /// <returns>Lista de productos cuyo precio se encuentra dentro del rango.</returns>
    public List<DomainProductEntity> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return _repository.FilterProductsByPriceRange(minPrice, maxPrice);
    }

    /// <summary>
    /// Obtiene productos con stock bajo según un umbral.
    /// </summary>
    /// <param name="stockThreshold">Umbral de stock para considerar un producto como de bajo stock.</param>
    /// <returns>Lista de productos con stock menor o igual al umbral.</returns>
    public List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold)
    {
        return _repository.GetProductsWithLowStock(stockThreshold);
    }
}