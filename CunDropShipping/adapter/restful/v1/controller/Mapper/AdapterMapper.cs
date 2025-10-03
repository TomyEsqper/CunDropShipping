using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.domain.Entity;

namespace CunDropShipping.adapter.restful.v1.controller.Mapper;

/// <summary>
/// Mapper que convierte entre las entidades del dominio (<see cref="DomainProductEntity"/>)
/// y las entidades expuestas por el adaptador/API (<see cref="AdapterProductEntity"/>).
/// </summary>
public class AdapterMapper : IAdapterMapper
{
    /// <summary>
    /// Convierte una entidad de dominio a su representación para la API.
    /// </summary>
    /// <param name="domainProduct">Entidad de dominio a convertir.</param>
    /// <returns>Instancia de <see cref="AdapterProductEntity"/> con los campos mapeados.</returns>
    public AdapterProductEntity ToAdapterProduct(DomainProductEntity domainProduct)
    {
        return new AdapterProductEntity
        {
            Id = domainProduct.Id,
            Name = domainProduct.Name,
            Description = domainProduct.Description,
            Price = domainProduct.Price,
            Stock = domainProduct.Stock,
        };    
    }

    /// <summary>
    /// Convierte una lista de entidades de dominio a una lista lista para la API.
    /// </summary>
    /// <param name="domainProducts">Lista de entidades del dominio.</param>
    /// <returns>Lista de <see cref="AdapterProductEntity"/> resultante.</returns>
    public List<AdapterProductEntity> ToAdapterProductList(List<DomainProductEntity> domainProducts)
    {
        return domainProducts.Count == 0 ? new List<AdapterProductEntity>() : domainProducts.Select(ToAdapterProduct).ToList();
    }

    /// <summary>
    /// Convierte una entidad del adaptador/API a su representación en el dominio.
    /// </summary>
    /// <param name="adapterProduct">Entidad del adaptador a convertir.</param>
    /// <returns>Instancia de <see cref="DomainProductEntity"/> con los campos mapeados.</returns>
    public DomainProductEntity ToDomainProduct(AdapterProductEntity adapterProduct)
    {
        return new DomainProductEntity
        {
            Id = adapterProduct.Id,
            Name = adapterProduct.Name,
            Description = adapterProduct.Description,
            Price = adapterProduct.Price,
            Stock = adapterProduct.Stock,
        }; 
    }

    /// <summary>
    /// Convierte una lista de entidades del adaptador a una lista de entidades del dominio.
    /// </summary>
    /// <param name="adapterProducts">Lista de entidades del adaptador.</param>
    /// <returns>Lista de <see cref="DomainProductEntity"/> resultante.</returns>
    public List<DomainProductEntity> ToDomeinProducts(List<AdapterProductEntity> adapterProducts)
    {
        return  adapterProducts.Count == 0 ? new List<DomainProductEntity>() : adapterProducts.Select(ToDomainProduct).ToList();   
    }
}