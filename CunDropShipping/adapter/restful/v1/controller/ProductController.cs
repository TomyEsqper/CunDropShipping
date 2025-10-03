using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.adapter.restful.v1.controller.Mapper;
using CunDropShipping.application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping.adapter.restful.v1.controller;

/// <summary>
/// Controlador REST para gestionar operaciones CRUD y consultas sobre productos.
/// Expone endpoints de la API en la ruta "api/v1/Products" y traduce entre
/// las entidades del adaptador (API) y las entidades del dominio mediante un mapper.
/// </summary>
[ApiController]
[Route("api/v1/Products")]
public class ProductController : ControllerBase
{   
    // 1. El recepcionista necesita hablar con el coordinador (IProducService
    private readonly IProductService _productService;

    private readonly IAdapterMapper _adapterMapper;
    // llamar al mapper construido para hacer la traduccion de datos
    
    /// <summary>
    /// Inicializa una nueva instancia de <see cref="ProductController" />.
    /// </summary>
    /// <param name="productService">Servicio de aplicación que contiene la lógica de negocio relacionada con productos.</param>
    /// <param name="adapterMapper">Mapper encargado de convertir entre entidades del adaptador (API) y entidades del dominio.</param>
    // 2. El constructor recibe al coordinado por inyeccion de dependencias
    public ProductController(IProductService productService, IAdapterMapper adapterMapper)
    {
        _productService = productService;
        _adapterMapper = adapterMapper;
    }
    
    /// <summary>
    /// Obtiene todos los productos disponibles.
    /// </summary>
    /// <returns>Un <see cref="ActionResult"/> que contiene una lista de <see cref="AdapterProductEntity"/> y un código HTTP 200 en caso de éxito.</returns>
    // 3. Define el primer método: Obtener todos los productos
    [HttpGet] // Esto responde a peticiones HTTP GET
    public ActionResult<List<AdapterProductEntity>> GetAllProducts()
    {
        // Llama al servicio para obtener la lista de productos (internos)

        // Usa el mapper de extensión para traducir la lista interna a una lista pública
        return Ok(_adapterMapper.ToAdapterProductList(_productService.GetAllProducts()));
    }

    /// <summary>
    /// Obtiene un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador único del producto a recuperar.</param>
    /// <returns>
    /// Un <see cref="ActionResult"/> que contiene la entidad <see cref="AdapterProductEntity"/> y un código HTTP 200 si se encuentra.
    /// Devuelve 404 NotFound si no existe el producto con el identificador proporcionado.
    /// </returns>
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

    /// <summary>
    /// Crea un nuevo producto a partir de la representación enviada por el cliente.
    /// </summary>
    /// <param name="product">Entidad de entrada en formato del adaptador que contiene los datos del producto a crear.</param>
    /// <returns>
    /// Un <see cref="ActionResult"/> que contiene la entidad creada <see cref="AdapterProductEntity"/> y un código HTTP 201 Created.
    /// Incluye la ubicación del recurso creado en la cabecera Location.
    /// </returns>
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

    /// <summary>
    /// Actualiza un producto existente identificado por su id.
    /// </summary>
    /// <param name="id">Identificador del producto a actualizar.</param>
    /// <param name="product">Entidad en formato del adaptador con los valores a actualizar.</param>
    /// <returns>
    /// Un <see cref="ActionResult"/> que contiene la entidad actualizada <see cref="AdapterProductEntity"/> y un código HTTP 200 si la actualización fue exitosa.
    /// Devuelve 404 NotFound si no existe el producto con el id proporcionado.
    /// </returns>
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

    /// <summary>
    /// Elimina un producto identificado por su id.
    /// </summary>
    /// <param name="id">Identificador del producto a eliminar.</param>
    /// <param name="product">Entidad en formato del adaptador que puede contener información adicional (por ejemplo para validaciones o auditoría).</param>
    /// <returns>
    /// Un <see cref="ActionResult"/> que contiene la entidad eliminada <see cref="AdapterProductEntity"/> y un código HTTP 200 si la eliminación fue exitosa.
    /// Devuelve 404 NotFound si no existe el producto con el id proporcionado.
    /// </returns>
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
    /// Busca productos por su nombre parcial o completo.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda que se utilizará para filtrar por nombre.</param>
    /// <returns>Una lista de <see cref="AdapterProductEntity"/> que coinciden con el término de búsqueda y un código HTTP 200.</returns>
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

    /// <summary>
    /// Filtra productos cuyo precio esté dentro de un rango especificado.
    /// </summary>
    /// <param name="minPrice">Precio mínimo inclusivo del rango.</param>
    /// <param name="maxPrice">Precio máximo inclusivo del rango.</param>
    /// <returns>Una lista de <see cref="AdapterProductEntity"/> que cumplen el rango de precio y un código HTTP 200.</returns>
    [HttpGet("filter/price")]
    public ActionResult<List<AdapterProductEntity>> FilterProductByPriceRange([FromQuery] decimal minPrice,
        [FromQuery] decimal maxPrice)
    {
        var domainProducts = _productService.FilterProductsByPriceRange(minPrice, maxPrice);
        var adapterProducts = _adapterMapper.ToAdapterProductList(domainProducts);
        return Ok(adapterProducts);
    }
    
    /// <summary>
    /// Obtiene productos cuyo stock está por debajo de un umbral definido.
    /// </summary>
    /// <param name="stockThreshold">Umbral de stock; se devolverán productos con stock menor o igual a este valor.</param>
    /// <returns>Una lista de <see cref="AdapterProductEntity"/> con stock bajo y un código HTTP 200.</returns>
    [HttpGet("stock/low")]
    public ActionResult<List<AdapterProductEntity>> GetProductsWithLowStock([FromQuery] int stockThreshold)
    {
        var domainProducts = _productService.GetProductsWithLowStock(stockThreshold);
        var adapterProducts = _adapterMapper.ToAdapterProductList(domainProducts);
        return Ok(adapterProducts);
    }
}