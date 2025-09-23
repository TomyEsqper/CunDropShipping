using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.Entity;
using CunDropShipping.infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace CunDropShipping.infrastructure.DbContext;

// Esta es la implementacion del Repositorio.
// Se encargara de todas las operaciones CRUD para la entidad ProductEntity.
public class IRepository
{
    // Guardamos una referencia a neutro "puente" con la base de datos.
    private readonly AppDbContext _context;
    
    // El constructor recibe el DbContext a traves de inyeccion de dependencias
    public IRepository(AppDbContext context)
    {
        _context = context;
    }
    
    // Este método promete devolver una lista de 'DomainProductEntity'.
    public async Task<IEnumerable<DomainProductEntity>> GetAll()
    {
        // 1. Obtenemos la lista de entidades de la base de datos.
        var productEntities = await _context.Products.ToListAsync();
        
        // 2, Usamos nuestro Mapper para traducir cada una de las entidades.
        // El '.Seect(p => p.ToDomain())' recorre la lista y aplica el metodo de traduccion a cada elemento.
        var domainProduct = productEntities.Select(p => p.ToDomain());
        
        // 3. Devolvemos la lista traducida.
        return domainProduct;   
    }
    
    // Aqui iran los metodos del CRUD
}