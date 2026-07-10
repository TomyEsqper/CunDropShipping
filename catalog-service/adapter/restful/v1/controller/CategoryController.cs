using Catalog.adapter.restful.v1.controller.Entity;
using Catalog.adapter.restful.v1.controller.Mapper;
using Catalog.application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping.adapter.restful.v1.controller;

[ApiController]
[Route("api/v1/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly ICategoryAdapterMapper _categoryAdapterMapper;

    public CategoryController(ICategoryService categoryService, ICategoryAdapterMapper categoryAdapterMapper)
    {
        _categoryService = categoryService;
        _categoryAdapterMapper = categoryAdapterMapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AdapterCategoryEntity>>> GetAllCategory()
    {
        var categories = await _categoryService.GetAllCategoryAsync();
        return Ok(_categoryAdapterMapper.ToAdapterCategoryList(categories));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdapterCategoryEntity>> GetCategoryById(int id)
    {
        var existingCategory = await _categoryService.GetCategoryByIdAsync(id);

        if (existingCategory == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        return Ok(_categoryAdapterMapper.ToAdapterCategory(existingCategory));
    }

    [HttpPost]
    public async Task<ActionResult<AdapterCategoryEntity>> SaveCategory([FromBody] AdapterCategoryEntity category)
    {
        var domainCategory = _categoryAdapterMapper.ToDomainCategory(category);
        var createdCategory = await _categoryService.SaveCategoryAsync(domainCategory);
        var adapterResult = _categoryAdapterMapper.ToAdapterCategory(createdCategory);

        return CreatedAtAction(nameof(GetCategoryById), new { id = adapterResult.CategoryId }, adapterResult);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<AdapterCategoryEntity>> UpdateCategory(int id, [FromBody] AdapterCategoryEntity category)
    {
        var domainCategory = _categoryAdapterMapper.ToDomainCategory(category);
        var updatedCategory = await _categoryService.UpdateCategoryAsync(id, domainCategory);

        if (updatedCategory == null)
        {
            return NotFound(new { message = $"Category with ID {id} not found" });
        }

        return Ok(_categoryAdapterMapper.ToAdapterCategory(updatedCategory));
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategory(int id)
    {
        try
        {
            var deletedCategory = await _categoryService.DeleteCategoryAsync(id);

            if (deletedCategory == null)
            {
                return NotFound(new { message = $"Category with ID {id} not found" });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
}
