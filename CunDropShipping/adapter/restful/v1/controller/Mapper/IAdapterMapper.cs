using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.domain.Entity;

namespace CunDropShipping.adapter.restful.v1.controller.Mapper;

public interface IAdapterMapper
{
    // Recibe una entidad de Dominio y devuelve una entidad del adaptador.
    AdapterProductEntity ToAdapterProduct(DomainProductEntity domainProduct);
    List<AdapterProductEntity> ToAdapterProductList(List<DomainProductEntity> domainProducts);
    
    // Recibe una entidad del Adaptador y la convierte a una entidad del Dominio.
    public DomainProductEntity ToDomainProduct(AdapterProductEntity adapterProduct);
    public List<DomainProductEntity> ToDomeinProducts(List<AdapterProductEntity> adapterProducts);

}