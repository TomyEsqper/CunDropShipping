using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.domain.Entity;

namespace CunDropShipping.adapter.restful.v1.controller.Mapper;

public interface IAdapterMapper
{
    // Contratos para traducir entre Domain y Adapter para un solo producto
    AdapterProductEntity ToAdapterProduct(DomainProductEntity domainProduct);
    DomainProductEntity ToDomainProduct(AdapterProductEntity adapterProduct);

    // Contratos para traducir listas de productos
    List<AdapterProductEntity> ToAdapterProductList(List<DomainProductEntity> domainProducts);
    //List<DomainProductEntity> ToDomainProductList(List<AdapterProductEntity> adapterProducts);

    // Contrato para traducir la solicitud de compra a un comando de dominio
    PurchaseCommand ToPurchaseCommand(PurchaseRequest request);

}