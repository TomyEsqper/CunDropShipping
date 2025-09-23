using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.domain.Entity;

namespace CunDropShipping.adapter.restful.v1.controller.Mapper;

public static class AdapterMapper
{
    // Este metodo convierte la entidad de Dominio (interna)
    // en la entidad del adaptador (publica).
    public static AdapterProductEntity ToAdapter(this DomainProductEntity domainEntity)
    {
        return new AdapterProductEntity
        {
            Id = domainEntity.Id,
            Name = domainEntity.Name,
            Description = domainEntity.Description,
            Price = domainEntity.Price,
            Stock = domainEntity.Stock,
        };
    }
    
    // Tambien creamos el traductor inverso para cuando necesitemos
    // convertir un formulario del cliente en una entidad de Dominio.
    // Lo usaremos par ael metodo de CREAR producto
    public static DomainProductEntity ToDomain(this AdapterProductEntity adapterEntity)
    {
        return new DomainProductEntity
        {
            // No mapeamos el Id porque al crear, la logica de negocio debe generarlo.
            Name = adapterEntity.Name,
            Description = adapterEntity.Description,
            Price = adapterEntity.Price,
            Stock = adapterEntity.Stock,
        };

    }
}