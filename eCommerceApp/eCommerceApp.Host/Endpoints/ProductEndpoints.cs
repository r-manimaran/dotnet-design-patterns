using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Services.Interfaces;

namespace eCommerceApp.Host.Endpoints;

public class ProductEndpoints
{
    public static void MapProductEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products").WithOpenApi().WithTags("Products");

        group.MapGet("/all", async (IProductService productService) =>
        {
            var products = await productService.GetAllAsync();
            return products.Any() ? Results.Ok(products) : Results.NotFound();
        }).WithName("GetProducts");

        group.MapGet("/{id}", async (Guid id, IProductService productService) =>
        {
            var product = await productService.GetByIdAsync(id);
            return Results.Ok(product);
        }).WithName("GetProduct");

        group.MapPost("/", async (CreateProduct product, IProductService productService) =>
        {
            var result = await productService.AddAsync(product);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).WithName("CreateProduct");

        group.MapPut("/", async (UpdateProduct product, IProductService productService) =>
        {
            var result = await productService.UpdateAsync(product);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).WithName("UpdateProduct");

        group.MapDelete("/{id}", async (Guid id, IProductService productService) =>
        {
            var result = await productService.DeleteAsync(id);
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        }).WithName("DeleteProduct");
    }
    
}
