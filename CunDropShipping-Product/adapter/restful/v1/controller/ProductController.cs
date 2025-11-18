using CunDropShipping.adapter.restful.v1.controller.Entity;
using CunDropShipping.adapter.restful.v1.controller.Mapper;
using CunDropShipping.application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping.adapter.restful.v1.controller
{
    [ApiController]
    // ¡SOLUCIÓN 1: Ruta en minúscula para coincidir con el Gateway!
    [Route("api/v1/products")] 
    public class ProductController : ControllerBase
    {   
        private readonly IProductService _productService;
        private readonly IAdapterMapper _adapterMapper;
        
        public ProductController(IProductService productService, IAdapterMapper adapterMapper)
        {
            _productService = productService;
            _adapterMapper = adapterMapper;
        }
        
        [HttpGet]
        public ActionResult<List<AdapterProductEntity>> GetAllProducts()
        {
            return Ok(_adapterMapper.ToAdapterProductList(_productService.GetAllProducts()));
        }

        [HttpGet("{id}")]
        public ActionResult<AdapterProductEntity> GetProductById(int id)
        {
            var product = _productService.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(_adapterMapper.ToAdapterProduct(product));
        }

        [HttpPost]
        public ActionResult<AdapterProductEntity> CreateProduct([FromBody] AdapterProductEntity product)
        {
            var domainProduct = _adapterMapper.ToDomainProduct(product);
            var createdProduct = _productService.SaveProduct(domainProduct);
            var adapterResult = _adapterMapper.ToAdapterProduct(createdProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = adapterResult.IdProduct }, adapterResult);
        }

        [HttpPut("{id}")]
        public ActionResult<AdapterProductEntity> UpdateProduct(int id, [FromBody] AdapterProductEntity product)
        {
            var domainProduct = _adapterMapper.ToDomainProduct(product);
            var updatedProduct = _productService.UpdateProduct(id, domainProduct);
            
            if (updatedProduct == null)
            {
                return NotFound();
            }
            
            var adapterResult = _adapterMapper.ToAdapterProduct(updatedProduct);
            return Ok(adapterResult);
        }

        /// <summary>
        /// Elimina un producto identificado por su id.
        /// </summary>
        // ¡SOLUCIÓN 2: Método DELETE limpio y corregido!
        [HttpDelete("{id}")]
        public ActionResult<AdapterProductEntity> DeleteProduct(int id)
        {
            // 1. Llama al servicio para borrar el producto
            var deletedProduct = _productService.DeleteProduct(id);
            
            // 2. Si devuelve null, es porque no lo encontró
            if (deletedProduct == null)
            {
                return NotFound(); // Devuelve 404 si no existe
            }
            
            // 3. (Como tu Gateway espera) Traduce el producto borrado y lo devuelve
            var adapterResult = _adapterMapper.ToAdapterProduct(deletedProduct);
            return Ok(adapterResult);
        }
        
        // --- ENDPOINTS FILTRADO AVANZADO ---
        
        [HttpGet("search")]
        public ActionResult<List<AdapterProductEntity>> SearchByName([FromQuery] string searchTerm)
        {
            var domainProducts = _productService.SearchProductsByName(searchTerm);
            var adapterProducts = _adapterMapper.ToAdapterProductList(domainProducts);
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
        
        [HttpGet("filter/stock")]
        public ActionResult<List<AdapterProductEntity>> GetProductsWithLowStock([FromQuery] int stockThreshold)
        {
            var domainProducts = _productService.GetProductsWithLowStock(stockThreshold);
            var adapterProducts = _adapterMapper.ToAdapterProductList(domainProducts);
            return Ok(adapterProducts);
        }
    }
}