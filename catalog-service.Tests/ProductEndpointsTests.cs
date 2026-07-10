using System.Net;
using System.Net.Http.Json;
using Catalog.adapter.restful.v1.controller.Entity;

namespace catalog_service.Tests;

[TestClass]
[DoNotParallelize]
public sealed class ProductEndpointsTests : IntegrationTestSupport
{
    [TestMethod]
    public async Task ProductCrudFlow_ReturnsExpectedResponses()
    {
        var category = await CreateCategoryAsync(Client, "product-category");
        var subCategory = await CreateSubCategoryAsync(Client, category.CategoryId, "product-subcategory");
        var created = await CreateProductAsync(Client, subCategory.SubCategoryId, "product");

        var getResponse = await Client.GetAsync($"/api/v1/products/{created.IdProduct}");
        Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<AdapterProductEntity>(JsonOptions);
        Assert.IsNotNull(fetched);
        Assert.AreEqual(created.IdProduct, fetched.IdProduct);
        Assert.AreEqual(created.NameProduct, fetched.NameProduct);

        created.NameProduct = $"{created.NameProduct}-updated";
        created.Description = "Updated integration test product";
        created.StockQuantity = 22;
        created.CurrentPrice = 88.50m;

        var updateResponse = await Client.PutAsJsonAsync($"/api/v1/products/{created.IdProduct}", created, JsonOptions);
        Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);

        var updated = await updateResponse.Content.ReadFromJsonAsync<AdapterProductEntity>(JsonOptions);
        Assert.IsNotNull(updated);
        Assert.AreEqual(created.NameProduct, updated.NameProduct);
        Assert.AreEqual(created.StockQuantity, updated.StockQuantity);

        var deleteResponse = await Client.DeleteAsync($"/api/v1/products/{created.IdProduct}");
        Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode);

        var missingResponse = await Client.GetAsync($"/api/v1/products/{created.IdProduct}");
        Assert.AreEqual(HttpStatusCode.NotFound, missingResponse.StatusCode);

        await Client.DeleteAsync($"/api/v1/subcategories/{subCategory.SubCategoryId}");
        await Client.DeleteAsync($"/api/v1/categories/{category.CategoryId}");
    }

    [TestMethod]
    public async Task ProductSearchAndFilters_RespectCreatedData()
    {
        var category = await CreateCategoryAsync(Client, "product-search-category");
        var subCategory = await CreateSubCategoryAsync(Client, category.CategoryId, "product-search-subcategory");
        var searchableProduct = await CreateProductAsync(Client, subCategory.SubCategoryId, "catalog-search-match", 120m, 3);
        var lowStockProduct = await CreateProductAsync(Client, subCategory.SubCategoryId, "catalog-low-stock", 50m, 1);
        var outOfRangeProduct = await CreateProductAsync(Client, subCategory.SubCategoryId, "catalog-out-of-range", 300m, 20);

        var searchResponse = await Client.GetAsync($"/api/v1/products/search?searchTerm={Uri.EscapeDataString("search-match")}");
        Assert.AreEqual(HttpStatusCode.OK, searchResponse.StatusCode);
        var searchResults = await searchResponse.Content.ReadFromJsonAsync<List<AdapterProductEntity>>(JsonOptions);
        Assert.IsNotNull(searchResults);
        Assert.IsTrue(searchResults.Any(product => product.IdProduct == searchableProduct.IdProduct));

        var priceResponse = await Client.GetAsync("/api/v1/products/filter/price?minPrice=100&maxPrice=150");
        Assert.AreEqual(HttpStatusCode.OK, priceResponse.StatusCode);
        var priceResults = await priceResponse.Content.ReadFromJsonAsync<List<AdapterProductEntity>>(JsonOptions);
        Assert.IsNotNull(priceResults);
        Assert.IsTrue(priceResults.Any(product => product.IdProduct == searchableProduct.IdProduct));
        Assert.IsFalse(priceResults.Any(product => product.IdProduct == outOfRangeProduct.IdProduct));

        var stockResponse = await Client.GetAsync("/api/v1/products/filter/stock?stockThreshold=3");
        Assert.AreEqual(HttpStatusCode.OK, stockResponse.StatusCode);
        var stockResults = await stockResponse.Content.ReadFromJsonAsync<List<AdapterProductEntity>>(JsonOptions);
        Assert.IsNotNull(stockResults);
        Assert.IsTrue(stockResults.Any(product => product.IdProduct == searchableProduct.IdProduct));
        Assert.IsTrue(stockResults.Any(product => product.IdProduct == lowStockProduct.IdProduct));
        Assert.IsFalse(stockResults.Any(product => product.IdProduct == outOfRangeProduct.IdProduct));

        await Client.DeleteAsync($"/api/v1/products/{searchableProduct.IdProduct}");
        await Client.DeleteAsync($"/api/v1/products/{lowStockProduct.IdProduct}");
        await Client.DeleteAsync($"/api/v1/products/{outOfRangeProduct.IdProduct}");
        await Client.DeleteAsync($"/api/v1/subcategories/{subCategory.SubCategoryId}");
        await Client.DeleteAsync($"/api/v1/categories/{category.CategoryId}");
    }
}
