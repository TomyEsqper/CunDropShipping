using Catalog.application.Service;
using Catalog.domain.Entity;
using Catalog.infrastructure.DbContext;

namespace Catalog.domain;

public class ProductServiceImp : IProductService
{
    private readonly ProductRepository _productRepository;
    private readonly ISubCategoryService _subCategoryService;

    public ProductServiceImp(ProductRepository productRepository, ISubCategoryService subCategoryService)
    {
        _productRepository = productRepository;
        _subCategoryService = subCategoryService;
    }

    public async Task<List<DomainProductEntity>> GetAllProductsAsync() => await _productRepository.GetAllProductsAsync();

    public async Task<DomainProductEntity?> GetProductByIdAsync(int id) => await _productRepository.GetProductByIdAsync(id);

    public async Task<DomainProductEntity> SaveProductAsync(DomainProductEntity product)
    {
        var subCat = await GetSubCategoryAsync(product.SubCategoryId);
        if (subCat == null) throw new KeyNotFoundException($"SubCategory with ID {product.SubCategoryId} not found");
        product.SubCategory = subCat;

        return await _productRepository.SaveProductAsync(product);
    }

    public async Task<DomainProductEntity?> UpdateProductAsync(int id, DomainProductEntity product)
    {
        var subCat = await GetSubCategoryAsync(product.SubCategoryId);
        if (subCat == null) throw new KeyNotFoundException($"SubCategory with ID {product.SubCategoryId} not found");

        product.SubCategory = subCat;
        return await _productRepository.UpdateProductAsync(id, product);
    }

    public async Task<DomainProductEntity?> DeleteProductAsync(int id) => await _productRepository.DeleteProductAsync(id);

    public async Task<List<DomainProductEntity>> SearchProductsByNameAsync(string term) => await _productRepository.SearchProductsByNameAsync(term);
    public async Task<List<DomainProductEntity>> FilterProductsByPriceRangeAsync(decimal min, decimal max) => await _productRepository.FilterProductsByPriceRangeAsync(min, max);
    public async Task<List<DomainProductEntity>> GetProductsWithLowStockAsync(int t) => await _productRepository.GetProductsWithLowStockAsync(t);

    private async Task<DomainSubCategoryEntity?> GetSubCategoryAsync(int subCategoryId) =>
        await _subCategoryService.GetSubCategoryByIdAsync(subCategoryId);
}
