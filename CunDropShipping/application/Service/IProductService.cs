//Importamos la entidad de dominio para poder usarla en nuestro contrato
using CunDropShipping.domain.Entity;

namespace CunDropShipping.application.Service;

// Esta es la definicion de nuestro contrato.
// Cualquier clase que se implemente esta interfaz esta OBLIGADA
// a tener estos metodos.

public interface IProductService
{
    // Contrato para obtener todos los productos
    // Metodos CRUD
    List<DomainProductEntity> GetAllProducts();
    DomainProductEntity GetProductById(int id);
    DomainProductEntity SaveProduct(DomainProductEntity product);
    DomainProductEntity UpdateProduct(int id, DomainProductEntity product);
    DomainProductEntity DeleteProduct(int id, DomainProductEntity product);
    
    // --- Metodos filtrado inteligente ---
    List<DomainProductEntity> SearchProductsByName(string searchTerm);
    List<DomainProductEntity> FilterProductsByPriceRange(decimal minPrice, decimal maxPrice);
    List<DomainProductEntity> GetProductsWithLowStock(int stockThreshold);
}