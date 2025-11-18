// Esta es la implementacion del Repositorio.
// Se encargara de todas las operaciones CRUD para la entidad ProductEntity.
using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.Entity;
using CunDropShipping.infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CunDropShipping.infrastructure.DbContext;

/// <summary>
/// Repositorio responsable de la persistencia de productos.
/// Implementa operaciones CRUD y consultas específicas sobre la tabla de productos usando Entity Framework Core.
/// </summary>
public class Repository
{
    // Guardamos una referencia a neutro "puente" con la base de datos.
    private readonly AppDbContext _context;
    private readonly IInfrastructureMapper _mapper;
    
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="Repository"/>.
    /// </summary>
    /// <param name="context">DbContext configurado para acceder a la base de datos.</param>
    /// <param name="mapper">Mapper encargado de convertir entre entidades de infraestructura y dominio.</param>
    public Repository(AppDbContext context, IInfrastructureMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los productos almacenados en la base de datos.
    /// </summary>
    /// <returns>Lista de <see cref="DomainProductEntity"/> representando los productos.</returns>
    public List<DomainProductEntity> GetAllProducts()
    {
        // 1. Obtiene los datos crudos de la BD.
        var productEntities = _context.Products
            .AsNoTracking()
            .OrderBy(p => p.IdProduct)
            .ToList();
        
        // 2. Usa el mapper para traducir y devolver el resultado.
        return _mapper.ToDomainProductEntityList(productEntities);
    }

    /// <summary>
    /// Obtiene un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto a buscar.</param>
    /// <returns>La entidad <see cref="DomainProductEntity"/> encontrada.</returns>
    /// <exception cref="KeyNotFoundException">Thrown cuando no se encuentra el producto.</exception>
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

    /// <summary>
    /// Crea y persiste un nuevo producto en la base de datos.
    /// </summary>
    /// <param name="domainProduct">Entidad de dominio con los datos del producto a guardar.</param>
    /// <returns>La entidad de dominio creada, con el Id generado por la base de datos.</returns>
    public DomainProductEntity SaveProduct(DomainProductEntity domainProduct)
    {
     // 1. Usa el mapper para traducir de Dominio a Infraestructura.
     var infraProduct = _mapper.ToInfrastructureEntity(domainProduct);
     
     // 2. anade la nueva entidad al DbContext. Aun no esta en la BD.
     _context.Products.Add(infraProduct);
     
     // 3. Confirma la trasaccion y guarda los cambio en la base de datos.
     _context.SaveChanges();
     
     // 4. EF actualiza el 'infraProduct' con el ID generado por la BD.
     // Lo traducimos de vuelta a Dominio y lo devolvemos.
        return _mapper.ToDomainProductEntity(infraProduct);
    }

    /// <summary>
    /// Actualiza un producto existente identificado por su id.
    /// </summary>
    /// <param name="id">Identificador del producto a actualizar.</param>
    /// <param name="domainProduct">Entidad de dominio con los nuevos valores.</param>
    /// <returns>La entidad de dominio actualizada o null si no existe el producto.</returns>
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
        existingProduct.nameProduct = domainProduct.NameProduct;
        existingProduct.Description = domainProduct.Description;
        existingProduct.price = domainProduct.Price;
        existingProduct.stockQuantity = domainProduct.StockQuantity;
        
        // 3. GUARDAR: Guardamos los cambios. Como EF esta "rastreando" a 'existingProduct',
        // sabe que debe generar un comando UPDATE, no un INSERT.
        _context.SaveChanges();
        
        // Devolvemos el producto ya actualizado y traducido.
        return _mapper.ToDomainProductEntity(existingProduct);
    }

    /// <summary>
    /// Elimina un producto identificado por su id.
    /// </summary>
    /// <param name="idProduct"></param>
    /// <returns>La entidad de dominio eliminada o null si no existe.</returns>
    public DomainProductEntity DeleteProduct(int idProduct)
    {
        // 1. BUSCAR: Primero, encontramos el producto que ya existe en la base de datos.
        var existingProduct = _context.Products.Find(idProduct);
        
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

    /// <summary>
    /// Busca productos cuyo nombre contiene el término de búsqueda.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda para filtrar por nombre.</param>
    /// <returns>Lista de entidades de dominio que coinciden con el término o null si el término está vacío.</returns>
    public List<DomainProductEntity> SearchProductsByName(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return null; // Devuelve una lista vacia si no hay termino.
        }
        
        var normalizedSearchTerm = searchTerm.Trim().ToLower();
        
        // 1. Busca en la base de datos y obtiene las entidades de infraestructura
        var foundInfraProduts = _context.Products
            .Where(p => p.nameProduct.ToLower().Contains(searchTerm))
            .ToList();
        
        // 2. Usa el mapper para traducir la lista y devolverla
        return _mapper.ToDomainProductEntityList(foundInfraProduts);
    }

    /// <summary>
    /// Filtra productos por un rango de precios.
    /// </summary>
    /// <param name="minPrice">Precio mínimo inclusivo.</param>
    /// <param name="maxPrice">Precio máximo inclusivo.</param>
    /// <returns>Lista de entidades de dominio que cumplen el rango de precio.</returns>
    public List<DomainProductEntity> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        var foundInfraProduts = _context.Products
            .Where(p => p.price >= minPrice && p.price <= maxPrice)
            .OrderBy(p => p.price)
            .ToList();

        return _mapper.ToDomainProductEntityList(foundInfraProduts);
    }

    /// <summary>
    /// Obtiene los productos cuyo stock es menor o igual al umbral especificado.
    /// </summary>
    /// <param name="stockThreshold">Umbral de stock para filtrar.</param>
    /// <returns>Lista de entidades de dominio con stock bajo.</returns>
    public List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold)
    {
        var foundIndraProducts = _context.Products
            .Where(p => p.stockQuantity <= stockThreshold)
            .OrderBy(p => p.stockQuantity)
            .ToList();
        return _mapper.ToDomainProductEntityList(foundIndraProducts);
    }
}