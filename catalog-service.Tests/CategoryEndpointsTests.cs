using System.Net;
using System.Net.Http.Json;
using Catalog.adapter.restful.v1.controller.Entity;

namespace catalog_service.Tests;

[TestClass]
[DoNotParallelize]
public sealed class CategoryEndpointsTests : IntegrationTestSupport
{
    [TestMethod]
    public async Task CategoryCrudFlow_ReturnsExpectedResponses()
    {
        var created = await CreateCategoryAsync(Client, "category");

        var getResponse = await Client.GetAsync($"/api/v1/categories/{created.CategoryId}");
        Assert.AreEqual(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<AdapterCategoryEntity>(JsonOptions);
        Assert.IsNotNull(fetched);
        Assert.AreEqual(created.CategoryId, fetched.CategoryId);
        Assert.AreEqual(created.NameCategory, fetched.NameCategory);

        created.NameCategory = $"{created.NameCategory}-updated";
        created.ProtectionDays = 45;

        var updateResponse = await Client.PutAsJsonAsync($"/api/v1/categories/{created.CategoryId}", created, JsonOptions);
        Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode);

        var updated = await updateResponse.Content.ReadFromJsonAsync<AdapterCategoryEntity>(JsonOptions);
        Assert.IsNotNull(updated);
        Assert.AreEqual(created.NameCategory, updated.NameCategory);
        Assert.AreEqual(created.ProtectionDays, updated.ProtectionDays);

        var deleteResponse = await Client.DeleteAsync($"/api/v1/categories/{created.CategoryId}");
        Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var missingResponse = await Client.GetAsync($"/api/v1/categories/{created.CategoryId}");
        Assert.AreEqual(HttpStatusCode.NotFound, missingResponse.StatusCode);
    }

    [TestMethod]
    public async Task DeleteCategory_WhenSubCategoriesExist_ReturnsConflict()
    {
        var category = await CreateCategoryAsync(Client, "category-conflict");
        var subCategory = await CreateSubCategoryAsync(Client, category.CategoryId, "subcategory-conflict");

        var deleteResponse = await Client.DeleteAsync($"/api/v1/categories/{category.CategoryId}");
        Assert.AreEqual(HttpStatusCode.Conflict, deleteResponse.StatusCode);

        await Client.DeleteAsync($"/api/v1/subcategories/{subCategory.SubCategoryId}");
        await Client.DeleteAsync($"/api/v1/categories/{category.CategoryId}");
    }
}
