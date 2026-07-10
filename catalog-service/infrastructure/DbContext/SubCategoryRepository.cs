using Catalog.domain.Entity;
using Catalog.infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Catalog.infrastructure.DbContext;

public class SubCategoryRepository
{
    private const string DeletedProductStatus = "DELETED";
    private readonly AppDbContext _context;
    private readonly ISubCategoryInfrastructureMapper _mapper;


    public SubCategoryRepository(AppDbContext context, ISubCategoryInfrastructureMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DomainSubCategoryEntity>> GetAllSubCategoryAsync()
    {
        var subCategories = await _context.SubCategories
            .AsNoTracking()
            .OrderBy(p => p.NameSubCategory)
            .ToListAsync();
        
        return _mapper.ToDomainSubCategoryEntityList(subCategories);
    }

    public async Task<DomainSubCategoryEntity?> GetSubCategoryByIdAsync(int id)
    {
        var infraSubCategory = await _context.SubCategories
            .FirstOrDefaultAsync(subCategory => subCategory.SubCategoryId == id);

        if (infraSubCategory == null)
        {
            return null;
        }
        
        return _mapper.ToDomainSubCategoryEntity(infraSubCategory);
    }

    public async Task<DomainSubCategoryEntity> SaveSubCategoryAsync(DomainSubCategoryEntity domainSubCategory)
    {
        var infraSubCategory = _mapper.ToInfrastructureSubCategoryEntity(domainSubCategory);

        await _context.SubCategories.AddAsync(infraSubCategory);
        
        await _context.SaveChangesAsync();
        
        return _mapper.ToDomainSubCategoryEntity(infraSubCategory);
        
    }

    public async Task<DomainSubCategoryEntity?> UpdateSubCategoryAsync(int id, DomainSubCategoryEntity domainSubCategory)
    {
        var existingSubCategory = await _context.SubCategories.FindAsync(id);
        if (existingSubCategory == null) return null;
        
        existingSubCategory.NameSubCategory = domainSubCategory.NameSubCategory;
        existingSubCategory.CategoryId = domainSubCategory.CategoryId;
        
        await _context.SaveChangesAsync();
        
        domainSubCategory.SubCategoryId = existingSubCategory.SubCategoryId;
        return domainSubCategory;
            
    }

    public async Task<DomainSubCategoryEntity?> DeleteSubCategoryAsync(int id)
    {
        var existingSubCategory = await _context.SubCategories.FindAsync(id);
        if (existingSubCategory == null) return null;

        var hasActiveProducts = await _context.Products
            .AnyAsync(product => product.SubCategoryId == id && product.ProductStatus != DeletedProductStatus);

        if (hasActiveProducts)
        {
            throw new InvalidOperationException("Cannot delete subcategory because it has active products.");
        }

        _context.SubCategories.Remove(existingSubCategory);

        await _context.SaveChangesAsync();

        return _mapper.ToDomainSubCategoryEntity(existingSubCategory);
    }
}
