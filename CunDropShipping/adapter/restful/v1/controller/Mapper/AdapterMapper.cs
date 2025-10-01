using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.domain.Entity;

namespace CunDropShipping.adapter.restful.v1.controller.Mapper;

public class AdapterMapper : IAdapterMapper
{
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

    public List<AdapterProductEntity> ToAdapterProductList(List<DomainProductEntity> domainProducts)
    {
        return domainProducts.Count == 0 ? new List<AdapterProductEntity>() : domainProducts.Select(ToAdapterProduct).ToList();
    }

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

    public List<DomainProductEntity> ToDomeinProducts(List<AdapterProductEntity> adapterProducts)
    {
        return  adapterProducts.Count == 0 ? new List<DomainProductEntity>() : adapterProducts.Select(ToDomainProduct).ToList();   
    }
}