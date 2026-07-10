using Catalog.adapter.restful.v1.controller.Entity;
using Catalog.adapter.restful.v1.controller.Mapper;
using Catalog.application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping.adapter.restful.v1.controller
{
    [ApiController]
    [Route("api/v1/products")] 
    public class ProductController : ControllerBase
    {   
        private readonly IProductService _productService;
        private readonly IProductAdapterMapper _productAdapterMapper;
        
        public ProductController(IProductService productService, IProductAdapterMapper productAdapterMapper)
        {
            _productService = productService;
            _productAdapterMapper = productAdapterMapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<AdapterProductEntity>>> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(_productAdapterMapper.ToAdapterProductList(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AdapterProductEntity>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }
            return Ok(_productAdapterMapper.ToAdapterProduct(product));
        }

        [HttpPost]
        public async Task<ActionResult<AdapterProductEntity>> CreateProduct([FromBody] AdapterProductEntity productDto)
        {
            var domainProduct = _productAdapterMapper.ToDomainProduct(productDto);
            if (domainProduct == null)
            {
                return BadRequest(new { message = "Product payload is required" });
            }

            try
            {
                var savedProduct = await _productService.SaveProductAsync(domainProduct);

                var adapterResult = _productAdapterMapper.ToAdapterProduct(savedProduct);
                if (adapterResult == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Product mapping failed" });
                }

                return CreatedAtAction(nameof(GetProductById), new { id = adapterResult.IdProduct }, adapterResult);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AdapterProductEntity>> UpdateProduct(int id, [FromBody] AdapterProductEntity productDto)
        {
            var domainProduct = _productAdapterMapper.ToDomainProduct(productDto);
            if (domainProduct == null)
            {
                return BadRequest(new { message = "Product payload is required" });
            }

            try
            {
                var updatedProduct = await _productService.UpdateProductAsync(id, domainProduct);

                if (updatedProduct == null)
                {
                    return NotFound(new { message = $"Product with ID {id} not found" });
                }

                return Ok(_productAdapterMapper.ToAdapterProduct(updatedProduct));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<AdapterProductEntity>> DeleteProduct(int id)
        {
            var deletedProduct = await _productService.DeleteProductAsync(id);
            
            if (deletedProduct == null)
            {
                return NotFound(new { message = $"Product with ID {id} not found" });
            }
            
            return Ok(_productAdapterMapper.ToAdapterProduct(deletedProduct));
        }
        
        // --- ENDPOINTS FILTRADO AVANZADO ---
        
        [HttpGet("search")]
        public async Task<ActionResult<List<AdapterProductEntity>>> SearchByName([FromQuery] string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest(new { message = "searchTerm is required" });
            }

            var domainProducts = await _productService.SearchProductsByNameAsync(searchTerm);
            return Ok(_productAdapterMapper.ToAdapterProductList(domainProducts)); 
        }

        [HttpGet("filter/price")]
        public async Task<ActionResult<List<AdapterProductEntity>>> FilterProductByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            if (minPrice > maxPrice)
            {
                return BadRequest(new { message = "minPrice cannot be greater than maxPrice" });
            }

            var domainProducts = await _productService.FilterProductsByPriceRangeAsync(minPrice, maxPrice);
            return Ok(_productAdapterMapper.ToAdapterProductList(domainProducts));
        }
        
        [HttpGet("filter/stock")]
        public async Task<ActionResult<List<AdapterProductEntity>>> GetProductsWithLowStock([FromQuery] int stockThreshold)
        {
            if (stockThreshold < 0)
            {
                return BadRequest(new { message = "stockThreshold cannot be negative" });
            }

            var domainProducts = await _productService.GetProductsWithLowStockAsync(stockThreshold);
            return Ok(_productAdapterMapper.ToAdapterProductList(domainProducts));
        }
    }
}
