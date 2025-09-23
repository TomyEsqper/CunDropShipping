using CunDropShipping.application.Service;
using CunDropShipping.adapter.restful.v1.controller.Mapper;
using CunDropShipping.domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping.adapter.restful.v1.controller;

[ApiController]
[Route("api/v1/Product")]
public class ProductController : ControllerBase
{   
    // 1. El recepcionista necesita hablar con el coordinador (IProducService
    private readonly IProductService _productService;
    
    // 2. El constructor recibe al coordinado por inyeccion de dependencias
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    
    // 3. Define el primer método: Obtener todos los productos
    [HttpGet] // Esto responde a peticiones HTTP GET
    public async Task<IActionResult> GetAllProducts()
    {
        // Llama al servicio para obtener la lista de productos (internos)
        var products = await _productService.GetAllAsync();

        // Usa el map
        // per para traducir la lista interna a una lista pública
        var productResponse = products.Select(p => p.ToAdapter());

        // Devuelve la respuesta con un código 200 OK y los datos.
        return Ok(productResponse);
    }
}