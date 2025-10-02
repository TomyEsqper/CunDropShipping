using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.Entity;
using CunDropShipping.infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CunDropShipping.infrastructure.DbContext;

// Esta es la implementacion del Repositorio.
// Se encargara de todas las operaciones CRUD para la entidad ProductEntity.
public class Repository
{
    // Guardamos una referencia a neutro "puente" con la base de datos.
    private readonly AppDbContext _context;
    private readonly IInfrastructureMapper _mapper;
    
    // El constructor recibe el DbContext a traves de inyeccion de dependencias
    public Repository(AppDbContext context, IInfrastructureMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // El método devuelve la lista YA TRADUCIDA a DomainProductEntity
    public List<DomainProductEntity> GetAllProducts()
    {
        // 1. Obtiene los datos crudos de la BD.
        var productEntities = _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToList();
        
        // 2. Usa el mapper para traducir y devolver el resultado.
        return _mapper.ToDomainProductEntityList(productEntities);
    }

    public DomainProductEntity GetProductById(int id)
    {
        // Busca directamente en la BD el producto con el ID especificado.
        var infraProduct = _context.Products.Find(id);
        
        // 2. Si no lo encuentra, podemos devolver null o lanzar un error.
        if (infraProduct == null)
        {
            throw new KeyNotFoundException("Product not found");
        }
        
        // 3. Si lo encuentra, lo traduce y lo devuelve.
        return _mapper.ToDomainProductEntity(infraProduct);
    }

    public DomainProductEntity SaveProduct(DomainProductEntity domainProduct)
    {
     // 1. Usa el mapper para traducir de Dominio a Infraestructura.
     var infraProduct = _mapper.ToInfrastructureEntity(domainProduct);
     
     // 2. anade la nueva entidad al DbContext. Aun no esta en la BD.
     _context.Products.Add(infraProduct);
     
     // 3. Confirma la trasaccion y guarda lso cambio en la base de  datos.
     _context.SaveChanges();
     
     // 4. EF actualiza el 'infraProduct' con el ID generado por la BD.
     // Lo traducimos de vuelta a Dominio y lo devolvemos.
        return _mapper.ToDomainProductEntity(infraProduct);
    }

    public DomainProductEntity UpdateProduct(int id,DomainProductEntity domainProduct)
    {
        // 1. BUSCAR: Primero, encontramos el producto que ya existe en la base de datos.
        var existingProduct = _context.Products.Find(id);
        
        // Si no lo encontramos, no podemos actualizarlo. Devolvemos null.
        if (existingProduct == null)
        {
            return null;
        }
        
        // 2. MODIFICAR: Actualizamos las propiedades del producto que encontramos.
        existingProduct.Name = domainProduct.Name;
        existingProduct.Description = domainProduct.Description;
        existingProduct.Price = domainProduct.Price;
        existingProduct.Stock = domainProduct.Stock;
        
        // 3. GUARDAR: Guardamos los cambios. Como EF esta "rastreando" a 'existingProduct',
        // sabe que debe generar un comando UPDATE, no un INSERT.
        _context.SaveChanges();
        
        // Devolvemos el producto ya actualizado y traducido.
        return _mapper.ToDomainProductEntity(existingProduct);
    }

    public DomainProductEntity DeleteProduct(int id, DomainProductEntity domainProduct)
    {
        // 1. BUSCAR: Primero, encontramos el producto que ya existe en la base de datos.
        var existingProduct = _context.Products.Find(id);
        
        // Si no lo encontramos, no podemos eliminarlo. Devolvemos null.
        if (existingProduct == null)
        {
            return null;
        }
        
        // 2. ELIMINAR: Lo marcamos para eliminacion.
        _context.Products.Remove(existingProduct);
        
        // 3. GUARDAR: Guardamos los cambios. EF generara un comando DELETE.
        _context.SaveChanges();
        
        // Devolvemos el producto que fue eliminado, traducido a Dominio.
        return _mapper.ToDomainProductEntity(existingProduct);
    }

    public List<DomainProductEntity> SearchProductsByName(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return null; // Devuelve una lista vacia si no hay termino.
        }
        
        var normalizedSearchTerm = searchTerm.Trim().ToLower();
        
        // 1. Busca en la base de datos y obtiene las entidades de infraestructura
        var foundInfraProduts = _context.Products
            .Where(p => p.Name.ToLower().Contains(searchTerm))
            .ToList();
        
        // 2. Usa el mapper para traducir la lista y devolverla
        return _mapper.ToDomainProductEntityList(foundInfraProduts);
    }

    public List<DomainProductEntity> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        var foundInfraProduts = _context.Products
            .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
            .OrderBy(p => p.Price)
            .ToList();

        return _mapper.ToDomainProductEntityList(foundInfraProduts);
    }

    public List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold)
    {
        var foundIndraProducts = _context.Products
            .Where(p => p.Stock <= stockThreshold)
            .OrderBy(p => p.Stock)
            .ToList();
        return _mapper.ToDomainProductEntityList(foundIndraProducts);
    }
}