// Improtamos las dos "fichas" que vamos a traducir entre si
using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.Entity;
namespace CunDropShipping.infrastructure.Mapper;

// La palabra 'static' es clave aqui. Significa que esta clase no se puede instanciar,
// solo sirve como un contenedor para nuestros metodos de traduccion.
public static class InfrastructureMapper
{
    // Metodo 1: Dela Base de DAtos HACIA el Dominio
    // Este metodo 'Extiende' la clase ProductEntity.
    // La palabra 'this' antes del parametro le da la habilidad de ser llamado como si fuera parte de la clase original.
    public static DomainProductEntity ToDomain(this ProductEntity insfrastructureEntity)
    {
        // Creamos una nueva entidad de Dominio...
        return new DomainProductEntity
        {
            // ... y copiamos los valores campo por campo.
            Id = insfrastructureEntity.Id,
            Name = insfrastructureEntity.Name,
            Description = insfrastructureEntity.Description,
            Price = insfrastructureEntity.Price,
            Stock = insfrastructureEntity.Stock
        };
    }
}