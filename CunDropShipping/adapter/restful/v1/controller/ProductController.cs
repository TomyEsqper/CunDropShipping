using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.adapter.restful.v1.controller.Mapper;
using CunDropShipping.application.Service;
using CunDropShipping.domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping.adapter.restful.v1.controller;

[ApiController]
[Route("api/v1/Products")]
public class ProductController : ControllerBase
{   
    // 1. El recepcionista necesita hablar con el coordinador (IProducService
    private readonly IProductService _productService;

    private readonly IAdapterMapper _adapterMapper;
    // llamar al mapper construido para hacer la traduccion de datos
    
    // 2. El constructor recibe al coordinado por inyeccion de dependencias
    public ProductController(IProductService productService, IAdapterMapper adapterMapper)
    {
        _productService = productService;
        _adapterMapper = adapterMapper;
    }
    
    // 3. Define el primer método: Obtener todos los productos
    [HttpGet] // Esto responde a peticiones HTTP GET
    public ActionResult<List<AdapterProductEntity>> GetAllProducts()
    {
        // Llama al servicio para obtener la lista de productos (internos)

        // Usa el mapper de extensión para traducir la lista interna a una lista pública
        return Ok(_adapterMapper.ToAdapterProductList(_productService.GetAllProducts()));
    }

    [HttpGet("{id}")]
    public ActionResult<AdapterProductEntity> GetProductById(int id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
        {
            // Si el servicio no encontro el producto, devolvemos un 404 Not Found.
            return NotFound();
        }
        
        // Si lo encontro, lo traducimos y lo devolvemos con un 200 OK
        return Ok(_adapterMapper.ToAdapterProduct(product));
    }

    [HttpPost]
    public ActionResult<AdapterProductEntity> CreateProduct([FromBody] AdapterProductEntity product)
    {
        // 1. Traduce del "Formulario del cliente" al "lenguaje de negocio".
        var domainProduct = _adapterMapper.ToDomainProduct(product);
        
        // 2. Llama a servicio para que guarde el producto.
        var CreatedProduct = _productService.SaveProduct(domainProduct);
        
        // 3. Traduce el producto guardado (que ya tiene Id) de vuelta al formato del cliente.
        var adapterResult = _adapterMapper.ToAdapterProduct(CreatedProduct);
        
        // 4. Devuelve una respuesta "201 Created", que es el estandar para un POST exitoso.
        //  Incluye la URL para encontrar el nuevo recurso y el objeto creado.
        return CreatedAtAction(nameof(GetProductById), new { id = adapterResult.Id }, adapterResult);
    }

    [HttpPut("{id}")]
    public ActionResult<AdapterProductEntity> UpdateProduct(int id, [FromBody] AdapterProductEntity product)
    {
        // Traducimos el "Formulario del cliente" al "lenguaje de negocio".
        var domainProduct = _adapterMapper.ToDomainProduct(product);
        
        // Llamamos al servicio para que intente actualizarlo.
        var updatedProduct = _productService.UpdateProduct(id, domainProduct);
        
        // Si el servicio devuelve nul, es porque no encontro el producto.
        if (updatedProduct == null)
        {
            return NotFound();
        }
        
        // Si t0do salio bien, traducimos el resultado y devolvemos un 200 OK.
        var adapterResult = _adapterMapper.ToAdapterProduct(updatedProduct);
        return Ok(adapterResult);
    }

    [HttpDelete("{id}")]
    public ActionResult<AdapterProductEntity> DeleteProduct(int id, [FromBody] AdapterProductEntity product)
    {
        var domainProduct = _adapterMapper.ToDomainProduct(product);

        var deletedProduct = _productService.DeleteProduct(id, domainProduct);
        
        if (deletedProduct == null)
        {
            return NotFound();
        }
        
        var adapterResult = _adapterMapper.ToAdapterProduct(deletedProduct);
        return Ok(adapterResult);
    }
    
    // --- ENDPOINTS FILTRADO AVANZADO ---
    
    /// <summary>
    /// Busca productos por su nombre.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda.</param>
    /// <returns>Una lista de productos coincidentes.</returns>
    [HttpGet("search")]
    public ActionResult<List<AdapterProductEntity>> SearchByName([FromQuery] string searchTerm)
    {
        // 1. Llama al servicio, que a su vez llama al repositorio.
        var domainProducts = _productService.SearchProductsByName(searchTerm);
        // 2. Traduce la lista de dominio a la lista para el "cliente" (la API).
        var adapterProducts = _adapterMapper.ToAdapterProductList(domainProducts);
        // 3. Devuelve la lista con un 200 OK.
        return Ok(adapterProducts); 
    }

    [HttpGet("filter/price")]
    public ActionResult<List<AdapterProductEntity>> FilterProductByPriceRange([FromQuery] decimal minPrice,
        [FromQuery] decimal maxPrice)
    {
        var domainProducts = _productService.FilterProductsByPriceRange(minPrice, maxPrice);
        var adapterProducts = _adapterMapper.ToAdapterProductList(domainProducts);
        return Ok(adapterProducts);
    }
    
    [HttpGet("stock/low")]
    public ActionResult<List<AdapterProductEntity>> GetProductsWithLowStock([FromQuery] int stockThreshold)
    {
        var domainProducts = _productService.GetProductsWithLowStock(stockThreshold);
        var adapterProducts = _adapterMapper.ToAdapterProductList(domainProducts);
        return Ok(adapterProducts);
    }
}