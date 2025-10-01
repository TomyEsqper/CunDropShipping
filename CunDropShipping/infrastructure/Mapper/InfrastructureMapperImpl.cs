// Improtamos las dos "fichas" que vamos a traducir entre si
using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.Entity;
namespace CunDropShipping.infrastructure.Mapper;

// La palabra 'static' es clave aqui. Significa que esta clase no se puede instanciar,
// solo sirve como un contenedor para nuestros metodos de traduccion.
public class InfrastructureMapperImpl : IInfrastructureMapper
{
    // Metodo 1: Dela Base de DAtos HACIA el Dominio
    // Este metodo 'Extiende' la clase ProductEntity.
    // La palabra 'this' antes del parametro le da la habilidad de ser llamado como si fuera parte de la clase original.
    public ProductEntity ToInfrastructureEntity(DomainProductEntity domainProduct)
    {
        return new ProductEntity
        {
            // ... y copiamos los valores campo por campo.
            Name = domainProduct.Name,
            Description = domainProduct.Description,
            Price = domainProduct.Price,
            Stock = domainProduct.Stock
        };      }

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

    public DomainProductEntity ToDomainProductEntity(ProductEntity domainProduct)
    {
        return new DomainProductEntity
        {
            // ... y copiamos los valores campo por campo.
            Id = domainProduct.Id,
            Name = domainProduct.Name,
            Description = domainProduct.Description,
            Price = domainProduct.Price,
            Stock = domainProduct.Stock
        };
    }

    public List<DomainProductEntity> ToDomainProductEntityList(List<ProductEntity> domainProduct)
    {
        if (domainProduct.Count() == 0)
        {
            return new List<DomainProductEntity>();
        }
        else
        {
            return domainProduct.Select(ToDomainProductEntity).ToList();
        }
    }
}