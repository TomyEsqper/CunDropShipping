// Añadimos los 'using' necesarios para el contrato y el repositorio
using CunDropShipping.application.Service;
using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.DbContext;

namespace CunDropShipping.domain;

// La clase sigue cumpliendo el mismo contrato (IProductService).
public class ProductServiceImp : IProductService
{
    // 1. EL ESPECIALISTA
    // Ya no tenemos una lista de juguete. Ahora tenemos una referencia
    // a nuestro "especialista en logistica", el IRepository.
    private readonly IRepository _repository;
    
    // 2. EL CONSTRCTOR (COMO RECIBE EL ESPECIALISTA)
    // Cuando la aplicacion cree este servicio, le "inyectara" automaticamente
    // una instacia del IRepository. El estratega no lo crea, lo recibe.
    public ProductServiceImp(IRepository repository)
    {
        _repository = repository;
    }
    
    // 3. LA TAREA DELGADA
    // Este metodo ahora es mucho mas simple.
    public async Task<List<DomainProductEntity>> GetAllAsync()
    {
        // Simplemente le pide al repositorio que obtenga todos los productos...
        var products = await _repository.GetAll();
        // ... y los devuelve como una lista
        return products.ToList();
    }
}