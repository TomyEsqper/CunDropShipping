using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.domain.Entity;

namespace CunDropShipping.adapter.restful.v1.controller.Mapper;

/// <summary>
/// Contrato para convertir entre las entidades del dominio y las entidades expuestas por la API (adaptador).
/// </summary>
public interface IAdapterMapper
{
    /// <summary>
    /// Convierte una entidad de dominio a su representación para la API.
    /// </summary>
    /// <param name="domainProduct">Entidad de dominio a convertir.</param>
    /// <returns>Instancia de <see cref="AdapterProductEntity"/> lista para exponer en la API.</returns>
    AdapterProductEntity ToAdapterProduct(DomainProductEntity domainProduct);

    /// <summary>
    /// Convierte una lista de entidades de dominio a una lista para la API.
    /// </summary>
    /// <param name="domainProducts">Lista de entidades del dominio.</param>
    /// <returns>Lista de <see cref="AdapterProductEntity"/> resultante.</returns>
    List<AdapterProductEntity> ToAdapterProductList(List<DomainProductEntity> domainProducts);
    
    /// <summary>
    /// Convierte una entidad enviada por la API a su representación en el dominio.
    /// </summary>
    /// <param name="adapterProduct">Entidad del adaptador a convertir.</param>
    /// <returns>Instancia de <see cref="DomainProductEntity"/> correspondiente.</returns>
    public DomainProductEntity ToDomainProduct(AdapterProductEntity adapterProduct);

    /// <summary>
    /// Convierte una lista de entidades del adaptador a una lista de entidades de dominio.
    /// </summary>
    /// <param name="adapterProducts">Lista de entidades del adaptador.</param>
    /// <returns>Lista de <see cref="DomainProductEntity"/> resultante.</returns>
    public List<DomainProductEntity> ToDomeinProducts(List<AdapterProductEntity> adapterProducts);

}