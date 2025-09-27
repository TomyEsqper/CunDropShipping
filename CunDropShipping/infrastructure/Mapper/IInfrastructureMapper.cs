using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.Entity;

namespace CunDropShipping.infrastructure.Mapper;

public interface IInfrastructureMapper
{
    ProductEntity ToInfrastructureEntity(DomainProductEntity domainProduct);
    List<ProductEntity> ToInfrastructureEntityList(List<DomainProductEntity> domainProductList);
    
    DomainProductEntity ToDomainProductEntity(ProductEntity domainProduct);
    List<DomainProductEntity> ToDomainProductEntityList(List<ProductEntity> domainProduct);
    
}