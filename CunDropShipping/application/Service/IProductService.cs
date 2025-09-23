//Importamos la entidad de dominio para poder usarla en nuestro contrato
using CunDropShipping.domain.Entity;

namespace CunDropShipping.application.Service;

// Esta es la definicion de nuestro contrato.
// Cualquier clase que se implemente esta interfaz esta OBLIGADA
// a tener estoss metodos.

public interface IProductService
{
    // Contrato para obtener todos los porductos
    Task<List<DomainProductEntity>> GetAllAsync();
    
    // Aquí añadiremos los otros contratos del CRUD más adelante
    // Task<DomainProductEntity> GetByIdAsync(Guid id);
    // Task<DomainProductEntity> CreateAsync(DomainProductEntity product);
    // Task<DomainProductEntity> UpdateAsync(Guid id, DomainProductEntity product);
    // Task DeleteAsync(Guid id);
}