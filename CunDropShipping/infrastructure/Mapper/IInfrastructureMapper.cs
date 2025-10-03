using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.Entity;

namespace CunDropShipping.infrastructure.Mapper;

/// <summary>
/// Contrato que define cómo mapear entre las entidades de infraestructura (BD) y las entidades de dominio.
/// Implementaciones concretas realizan la conversión entre ambos modelos.
/// </summary>
public interface IInfrastructureMapper
{
    /// <summary>
    /// Convierte una entidad de dominio a su representación de infraestructura (DB).
    /// </summary>
    /// <param name="domainProduct">Entidad de dominio con los datos del producto.</param>
    /// <returns>Instancia de <see cref="ProductEntity"/> lista para persistir en la base de datos.</returns>
    ProductEntity ToInfrastructureEntity(DomainProductEntity domainProduct);

    /// <summary>
    /// Convierte una lista de entidades de dominio a su representación de infraestructura.
    /// </summary>
    /// <param name="domainProductList">Lista de entidades de dominio.</param>
    /// <returns>Lista de <see cref="ProductEntity"/>.</returns>
    List<ProductEntity> ToInfrastructureEntityList(List<DomainProductEntity> domainProductList);
    
    /// <summary>
    /// Convierte una entidad de infraestructura (BD) al modelo de dominio.
    /// </summary>
    /// <param name="domainProduct">Entidad de infraestructura obtenida de la base de datos.</param>
    /// <returns>Instancia de <see cref="DomainProductEntity"/> correspondiente.</returns>
    DomainProductEntity ToDomainProductEntity(ProductEntity domainProduct);

    /// <summary>
    /// Convierte una lista de entidades de infraestructura a su representación de dominio.
    /// </summary>
    /// <param name="productEntities">Lista de entidades de infraestructura.</param>
    /// <returns>Lista de <see cref="DomainProductEntity"/>.</returns>
    List<DomainProductEntity> ToDomainProductEntityList(List<ProductEntity> productEntities);
    
}