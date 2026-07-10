using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Catalog.adapter.restful.v1.controller.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace catalog_service.Tests;

public abstract class IntegrationTestSupport : IDisposable
{
    protected static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters =
        {
            new JsonStringEnumConverter()
        }
    };

    private readonly IntegrationWebApplicationFactory _factory;

    protected readonly HttpClient Client;

    protected IntegrationTestSupport()
    {
        _factory = new IntegrationWebApplicationFactory();
        Client = _factory.CreateClient();
    }

    protected static string UniqueSuffix() => Guid.NewGuid().ToString("N");

    protected static async Task<AdapterCategoryEntity> CreateCategoryAsync(HttpClient client, string namePrefix)
    {
        var payload = new AdapterCategoryEntity
        {
            NameCategory = $"{namePrefix}-{UniqueSuffix()}",
            ProtectionDays = 30,
            CategoryStatus = Catalog.domain.Entity.CategoryStatus.Active
        };

        var response = await client.PostAsJsonAsync("/api/v1/categories", payload, JsonOptions);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<AdapterCategoryEntity>(JsonOptions))!;
    }

    protected static async Task<AdapterSubCategoryEntity> CreateSubCategoryAsync(HttpClient client, int categoryId, string namePrefix)
    {
        var payload = new AdapterSubCategoryEntity
        {
            NameSubCategory = $"{namePrefix}-{UniqueSuffix()}",
            CategoryId = categoryId
        };

        var response = await client.PostAsJsonAsync("/api/v1/subcategories", payload, JsonOptions);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<AdapterSubCategoryEntity>(JsonOptions))!;
    }

    protected static async Task<AdapterProductEntity> CreateProductAsync(HttpClient client, int subCategoryId, string namePrefix, decimal price = 99.99m, int stock = 10)
    {
        var payload = new AdapterProductEntity
        {
            SellerId = Guid.NewGuid(),
            SubCategoryId = subCategoryId,
            Sku = $"SKU-{UniqueSuffix()}",
            NameProduct = $"{namePrefix}-{UniqueSuffix()}",
            Description = "Integration test product",
            Price = price,
            CurrentPrice = price,
            StockQuantity = stock,
            ProductStatus = Catalog.domain.Entity.ProductStatus.Active
        };

        var response = await client.PostAsJsonAsync("/api/v1/products", payload, JsonOptions);
        response.EnsureSuccessStatusCode();

        return (await response.Content.ReadFromJsonAsync<AdapterProductEntity>(JsonOptions))!;
    }

    public void Dispose()
    {
        Client.Dispose();
        _factory.Dispose();
    }

    private sealed class IntegrationWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
        }
    }
}
