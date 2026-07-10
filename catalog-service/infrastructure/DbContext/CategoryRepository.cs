using Catalog.domain.Entity;
using Catalog.infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Catalog.infrastructure.DbContext;

public class CategoryRepository
{
    private const string DeletedStatus = "DELETED";
    private readonly AppDbContext _context;
    private readonly ICategoryInfrastructureMapper _mapper;
    
    public CategoryRepository(AppDbContext context, ICategoryInfrastructureMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DomainCategoryEntity>> GetAllCategoryAsync()
    {
        var categories = await _context.Categories
            .AsNoTracking()
            .Where(category => category.CategoryStatus != DeletedStatus)
            .OrderBy(c => c.NameCategory)
            .ToListAsync();

        return _mapper.ToDomainCategoryEntityList(categories);
    }
    
    public async Task<DomainCategoryEntity?> GetCategoryByIdAsync(int id)
    {
        var infraCategory = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(category => category.CategoryId == id && category.CategoryStatus != DeletedStatus);
        
        if (infraCategory == null) return null;
        
        return _mapper.ToDomainCategoryEntity(infraCategory);
    }

    public async Task<DomainCategoryEntity> SaveCategoryAsync(DomainCategoryEntity domainCategory)
    {
        var infraCategory = _mapper.ToInfrastructureCategoryEntity(domainCategory);
        
        await _context.Categories.AddAsync(infraCategory);
        await _context.SaveChangesAsync();
        
        return _mapper.ToDomainCategoryEntity(infraCategory);
    }

    public async Task<DomainCategoryEntity?> UpdateCategoryAsync(int id, DomainCategoryEntity domainCategory)
    {
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(category => category.CategoryId == id && category.CategoryStatus != DeletedStatus);
        if (existingCategory == null) return null;
        
        existingCategory.NameCategory = domainCategory.NameCategory;
        existingCategory.ProtectionDays = domainCategory.ProtectionDays;
        existingCategory.CategoryStatus = CategoryInfrastructureMapperImpl.StatusToDb(domainCategory.CategoryStatus);
        
        await _context.SaveChangesAsync();
        
        domainCategory.CategoryId = existingCategory.CategoryId;
        return domainCategory;
    }

    public async Task<DomainCategoryEntity?> DeleteCategoryAsync(int id)
    {
        var existingCategory = await _context.Categories
            .FirstOrDefaultAsync(category => category.CategoryId == id && category.CategoryStatus != DeletedStatus);
        if (existingCategory == null) return null;

        var hasSubCategories = await _context.SubCategories
            .AnyAsync(subCategory => subCategory.CategoryId == id);

        if (hasSubCategories)
        {
            throw new InvalidOperationException("Cannot delete category because it has subcategories.");
        }

        existingCategory.CategoryStatus = DeletedStatus;

        await _context.SaveChangesAsync();

        return _mapper.ToDomainCategoryEntity(existingCategory);
    }
}
