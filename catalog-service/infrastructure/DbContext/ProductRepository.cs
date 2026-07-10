using Catalog.domain.Entity;
using Catalog.infrastructure.Mapper;
using Microsoft.EntityFrameworkCore;

namespace Catalog.infrastructure.DbContext;

public class ProductRepository
{
    private const string DeletedStatus = "DELETED";
    private readonly AppDbContext _context;
    private readonly IProductInfrastructureMapper _mapper;
    
    public ProductRepository(AppDbContext context, IProductInfrastructureMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<DomainProductEntity>> GetAllProductsAsync()
    {
        var productEntities = await _context.Products
            .AsNoTracking()
            .Include(p => p.SubCategory)
            .Where(p => p.ProductStatus != DeletedStatus)
            .ToListAsync();
        return _mapper.ToDomainProductEntityList(productEntities);
    }

    public async Task<DomainProductEntity?> GetProductByIdAsync(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.SubCategory)
            .FirstOrDefaultAsync(p => p.IdProduct == id && p.ProductStatus != DeletedStatus);
        return _mapper.ToDomainProductEntity(product);
    }

    public async Task<DomainProductEntity> SaveProductAsync(DomainProductEntity domainProduct)
    {
        var infraProduct = _mapper.ToInfrastructureEntity(domainProduct);
        await _context.Products.AddAsync(infraProduct);
        await _context.SaveChangesAsync();
        domainProduct.IdProduct = infraProduct.IdProduct;
        return domainProduct;
    }

    public async Task<DomainProductEntity?> UpdateProductAsync(int id, DomainProductEntity domainProduct)
    {
        var existingEntity = await _context.Products
            .FirstOrDefaultAsync(p => p.IdProduct == id && p.ProductStatus != DeletedStatus);
        if (existingEntity == null) return null;

        existingEntity.NameProduct = domainProduct.NameProduct;
        existingEntity.Description = domainProduct.Description;
        existingEntity.Price = domainProduct.Price;
        existingEntity.CurrentPrice = domainProduct.CurrentPrice;
        existingEntity.StockQuantity = domainProduct.StockQuantity;
        existingEntity.SubCategoryId = domainProduct.SubCategoryId;
        existingEntity.SellerId = domainProduct.SellerId;
        existingEntity.Sku = domainProduct.Sku;
        existingEntity.ProductStatus = MapStatusToDb(domainProduct.ProductStatus);
        existingEntity.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return _mapper.ToDomainProductEntity(existingEntity);
    }

    public async Task<DomainProductEntity?> DeleteProductAsync(int idProduct)
    {
        var product = await _context.Products
            .Include(p => p.SubCategory)
            .FirstOrDefaultAsync(p => p.IdProduct == idProduct && p.ProductStatus != DeletedStatus);

        if (product == null)
        {
            return null;
        }

        product.ProductStatus = DeletedStatus;
        product.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();

        return _mapper.ToDomainProductEntity(product);
    }

    public async Task<List<DomainProductEntity>> SearchProductsByNameAsync(string searchTerm) =>
        _mapper.ToDomainProductEntityList(await _context.Products
            .AsNoTracking()
            .Include(p => p.SubCategory)
            .Where(p => p.ProductStatus != DeletedStatus && p.NameProduct.Contains(searchTerm))
            .ToListAsync());

    public async Task<List<DomainProductEntity>> FilterProductsByPriceRangeAsync(decimal min, decimal max) =>
        _mapper.ToDomainProductEntityList(await _context.Products
            .AsNoTracking()
            .Include(p => p.SubCategory)
            .Where(p => p.ProductStatus != DeletedStatus && p.Price >= min && p.Price <= max)
            .ToListAsync());

    public async Task<List<DomainProductEntity>> GetProductsWithLowStockAsync(int threshold) =>
        _mapper.ToDomainProductEntityList(await _context.Products
            .AsNoTracking()
            .Include(p => p.SubCategory)
            .Where(p => p.ProductStatus != DeletedStatus && p.StockQuantity <= threshold)
            .ToListAsync());

    private static string MapStatusToDb(ProductStatus status) => status switch
    {
        ProductStatus.Draft => "DRAFT",
        ProductStatus.Active => "ACTIVE",
        ProductStatus.Inactive => "INACTIVE",
        ProductStatus.OutOfStock => "OUT_OF_STOCK",
        ProductStatus.Banned => "BANNED",
        ProductStatus.Deleted => DeletedStatus,
        _ => "DRAFT"
    };
}
