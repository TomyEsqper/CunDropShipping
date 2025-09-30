using System.Collections.Generic; 
using CunDropShipping.domain.Entity;

namespace CunDropShipping.application.Service;

// Esta es la definicion de nuestro contrato.
// Cualquier clase que se implemente esta interfaz está OBLIGADA
// a tener estos metodos.

public interface IProductService
{
    // Contrato para obtener todos los productos
    List<DomainProductEntity> GetAllProducts();
    DomainProductEntity GetProductById(int id);
    DomainProductEntity SaveProduct(DomainProductEntity product);
    DomainProductEntity UpdateProduct(int id, DomainProductEntity product);
    DomainProductEntity DeleteProduct(int id, DomainProductEntity product);
    void ProcessPurchase(PurchaseCommand purchaseCommand);
}