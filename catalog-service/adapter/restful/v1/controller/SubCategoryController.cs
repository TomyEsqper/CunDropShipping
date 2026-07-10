using Catalog.adapter.restful.v1.controller.Entity;
using Catalog.adapter.restful.v1.controller.Mapper;
using Catalog.application.Service;
using Microsoft.AspNetCore.Mvc;

namespace CunDropShipping.adapter.restful.v1.controller;

[ApiController]
[Route("api/v1/subcategories")]
public class SubCategoryController : ControllerBase
{
    
    private readonly ISubCategoryService _subCategoryService;
    private readonly ISubCategoryAdapterMapper _subCategoryAdapterMapper;
    
    public SubCategoryController(ISubCategoryService subCategoryService, ISubCategoryAdapterMapper subCategoryAdapterMapper)
    {
        _subCategoryService = subCategoryService;
        _subCategoryAdapterMapper = subCategoryAdapterMapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<AdapterSubCategoryEntity>>> GetAllSubCategory()
    {
        var subCategories = await _subCategoryService.GetAllSubCategoryAsync();
        return Ok(_subCategoryAdapterMapper.ToAdapterSubCategoryList(subCategories));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AdapterSubCategoryEntity>> GetSubCategoryById(int id)
    {
        var existingSubCtegory = await _subCategoryService.GetSubCategoryByIdAsync(id);
        if (existingSubCtegory == null)
        {
            return NotFound(new { message = $"SubCategory with ID {id} not found" });
        }

        return Ok(_subCategoryAdapterMapper.ToAdapterSubCategory(existingSubCtegory));
    }
    
    [HttpPost]
    public async Task<ActionResult<AdapterSubCategoryEntity>> SaveSubCategory([FromBody] AdapterSubCategoryEntity subCategory)
    {
        var domainSubCategory = _subCategoryAdapterMapper.ToDomainSubCategory(subCategory);
        var createdSubCategory = await _subCategoryService.SaveSubCategoryAsync(domainSubCategory);
        var adapterResult = _subCategoryAdapterMapper.ToAdapterSubCategory(createdSubCategory);
        
        return CreatedAtAction(nameof(GetSubCategoryById), new { id = adapterResult.SubCategoryId }, adapterResult);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<AdapterSubCategoryEntity>> UpdateSubCategory(int id,
        [FromBody] AdapterSubCategoryEntity subCategory)
    {
        var domainSubCategory = _subCategoryAdapterMapper.ToDomainSubCategory(subCategory);
        var updatedSubCategory = await _subCategoryService.UpdateSubCategoryAsync(id, domainSubCategory);

        if (updatedSubCategory == null)
        {
            return NotFound(new { message = $"SubCategory with ID {id} not found" });
        }

        return Ok(_subCategoryAdapterMapper.ToAdapterSubCategory(updatedSubCategory));
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSubCategory(int id)
    {
        try
        {
            var deletedSubCategory = await _subCategoryService.DeleteSubCategoryAsync(id);

            if (deletedSubCategory == null)
            {
                return NotFound(new { message = $"SubCategory with ID {id} not found" });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }
    
    
}
