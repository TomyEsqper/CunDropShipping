// Improtamos las dos "fichas" que vamos a traducir entre si
using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.Entity;
namespace CunDropShipping.infrastructure.Mapper;

/// <summary>
/// Implementación del <see cref="IInfrastructureMapper"/> que convierte entre
/// las entidades de infraestructura (ProductEntity) y las entidades de dominio (DomainProductEntity).
/// </summary>
public class InfrastructureMapperImpl : IInfrastructureMapper
{
    /// <summary>
    /// Convierte una entidad de dominio a su representación de infraestructura.
    /// </summary>
    /// <param name="domainProduct">Entidad de dominio a convertir.</param>
    /// <returns>Instancia de <see cref="ProductEntity"/> con los campos mapeados.</returns>
    public ProductEntity ToInfrastructureEntity(DomainProductEntity domainProduct)
    {
        return new ProductEntity
        {
            // ... y copiamos los valores campo por campo.
            nameProduct = domainProduct.NameProduct,
            Description = domainProduct.Description,
            price = domainProduct.Price,
            stockQuantity = domainProduct.StockQuantity
        };      
    }

    /// <summary>
    /// Convierte una lista de entidades de dominio a una lista de entidades de infraestructura.
    /// </summary>
    /// <param name="domainProductList">Lista de entidades de dominio.</param>
    /// <returns>Lista de <see cref="ProductEntity"/> resultante de la conversión.</returns>
    public List<ProductEntity> ToInfrastructureEntityList(List<DomainProductEntity> domainProductList)
    {
        if (domainProductList.Count() == 0)
        {
            return new List<ProductEntity>();
        }
        else
        {
            return domainProductList.Select(ToInfrastructureEntity).ToList();
        }    }

    /// <summary>
    /// Convierte una entidad de infraestructura a su representación de dominio.
    /// </summary>
    /// <param name="domainProduct">Entidad de infraestructura a convertir.</param>
    /// <returns>Instancia de <see cref="DomainProductEntity"/> con los campos mapeados.</returns>
    public DomainProductEntity ToDomainProductEntity(ProductEntity domainProduct)
    {
        return new DomainProductEntity
        {
            // ... y copiamos los valores campo por campo.
            IdProduct = domainProduct.IdProduct,
            NameProduct = domainProduct.nameProduct,
            Description = domainProduct.Description,
            Price = domainProduct.price,
            StockQuantity = domainProduct.stockQuantity
        };
    }

    /// <summary>
    /// Convierte una lista de entidades de infraestructura a una lista de entidades de dominio.
    /// </summary>
    /// <param name="productEntities">Lista de entidades de infraestructura.</param>
    /// <returns>Lista de <see cref="DomainProductEntity"/> resultante de la conversión.</returns>
    public List<DomainProductEntity> ToDomainProductEntityList(List<ProductEntity> productEntities)
    {
        
        // Verifica si la lista de entrada es nula para evitar errores.
        if (productEntities.Count() == 0)
        {
            return new List<DomainProductEntity>();
        }
        // Convierte cada ProductEntity a DomainProductEntity y devolvemos la nueva lista.
        return productEntities.Select(p => ToDomainProductEntity(p)).ToList();
    }
}