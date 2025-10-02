// Añadimos los 'using' necesarios para el contrato y el repositorio
using CunDropShipping.application.Service;
using CunDropShipping.domain.Entity;
using CunDropShipping.infrastructure.DbContext;
using CunDropShipping.infrastructure.Mapper;

namespace CunDropShipping.domain;

// La clase sigue cumpliendo el mismo contrato (IProductService).
public class ProductServiceImp : IProductService
{
    // 1. EL ESPECIALISTA
    // Ya no tenemos una lista de juguete. Ahora tenemos una referencia
    // a nuestro "especialista en logistica", el IRepository.
    private readonly Repository _repository;

    // 2. EL CONSTRCTOR (COMO RECIBE EL ESPECIALISTA)
    // Cuando la aplicacion cree este servicio, le "inyectara" automaticamente
    // una instacia del IRepository. El estratega no lo crea, lo recibe.
    public ProductServiceImp(Repository set)
    {
        _repository = set;
    }

    // El service simplemente le pasa la orden al Repository.
    public List<DomainProductEntity> GetAllProducts()
    {
        return _repository.GetAllProducts();
    }

    public DomainProductEntity GetProductById(int id)
    {
        return _repository.GetProductById(id);
    }

    public DomainProductEntity SaveProduct(DomainProductEntity product)
    {
        return _repository.SaveProduct(product);
    }

    public DomainProductEntity UpdateProduct(int id, DomainProductEntity product)
    {
        return _repository.UpdateProduct(id, product);
    }

    public DomainProductEntity DeleteProduct(int id, DomainProductEntity product)
    {
        return _repository.DeleteProduct(id, product);
    }

    public List<DomainProductEntity> SearchProductsByName(string searchTerm)
    {
        return _repository.SearchProductsByName(searchTerm);
    }

    public List<DomainProductEntity> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice)
    {
        return _repository.FilterProductsByPriceRange(minPrice, maxPrice);
    }

    public List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold)
    {
        return _repository.GetProductsWithLowStock(stockThreshold);
    }
}