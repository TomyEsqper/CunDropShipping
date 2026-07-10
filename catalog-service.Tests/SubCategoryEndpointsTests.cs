using System.Net;
using System.Net.Http.Json;
using Catalog.adapter.restful.v1.controller.Entity;

namespace catalog_service.Tests;

[TestClass]
[DoNotParallelize]
public sealed class SubCategoryEndpointsTests : IntegrationTestSupport
{
    [TestMethod]
    public async Task SubCategoryCrudFlow_ReturnsExpectedResponses()
    {
        var category = await CreateCategoryAsync(Client, "subcategory-category");
        var created = await CreateSubCategoryAsync(Client, category.CategoryId, "subcategory");

        var getResponse = await Client.GetAsync($"/api/v1/subcategories/{created.SubCategoryId}");
        Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<AdapterSubCategoryEntity>(JsonOptions);
        Assert.IsNotNull(fetched);
        Assert.AreEqual(created.SubCategoryId, fetched.SubCategoryId);
        Assert.AreEqual(created.NameSubCategory, fetched.NameSubCategory);

        created.NameSubCategory = $"{created.NameSubCategory}-updated";
        created.CategoryId = category.CategoryId;

        var updateResponse = await Client.PutAsJsonAsync($"/api/v1/subcategories/{created.SubCategoryId}", created, JsonOptions);
        Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);

        var updated = await updateResponse.Content.ReadFromJsonAsync<AdapterSubCategoryEntity>(JsonOptions);
        Assert.IsNotNull(updated);
        Assert.AreEqual(created.NameSubCategory, updated.NameSubCategory);

        var deleteResponse = await Client.DeleteAsync($"/api/v1/subcategories/{created.SubCategoryId}");
        Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var missingResponse = await Client.GetAsync($"/api/v1/subcategories/{created.SubCategoryId}");
        Assert.AreEqual(HttpStatusCode.NotFound, missingResponse.StatusCode);

        await Client.DeleteAsync($"/api/v1/categories/{category.CategoryId}");
    }

    [TestMethod]
    public async Task DeleteSubCategory_WhenProductsExist_ReturnsConflict()
    {
        var category = await CreateCategoryAsync(Client, "subcategory-conflict-category");
        var subCategory = await CreateSubCategoryAsync(Client, category.CategoryId, "subcategory-conflict");
        var product = await CreateProductAsync(Client, subCategory.SubCategoryId, "product-for-subcategory-conflict");

        var deleteResponse = await Client.DeleteAsync($"/api/v1/subcategories/{subCategory.SubCategoryId}");
        Assert.AreEqual(HttpStatusCode.Conflict, deleteResponse.StatusCode);

        await Client.DeleteAsync($"/api/v1/products/{product.IdProduct}");
        await Client.DeleteAsync($"/api/v1/subcategories/{subCategory.SubCategoryId}");
        await Client.DeleteAsync($"/api/v1/categories/{category.CategoryId}");
    }
}
