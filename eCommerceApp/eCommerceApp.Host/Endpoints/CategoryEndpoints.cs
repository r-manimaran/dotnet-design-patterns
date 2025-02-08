using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Application.Services.Interfaces;

namespace eCommerceApp.Host.Endpoints
{
    public class CategoryEndpoints
    {
        public static void MapCategoryEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/categories").WithOpenApi().WithTags("Category");

            group.MapGet("/", async (ICategoryService categoryService) =>
            {
                var categories = await categoryService.GetAllAsync();
                return categories.Any() ? Results.Ok(categories) : Results.NotFound();
            }).WithName("GetCategories");

            group.MapGet("/{id}", async (Guid id, ICategoryService categoryService) =>
            {
                var result = await categoryService.GetByIdAsync(id);
                return Results.Ok(result);
            }).WithName("GetCategory");

            group.MapPost("/", async (CreateCategory category, ICategoryService categoryService) =>
            {
                //Add Fluent validation 

                var result = await categoryService.AddAsync(category);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            }).WithName("CreateCategory");

            group.MapPut("/", async (UpdateCategory category, ICategoryService categoryService) =>
            {
                // Add Fluent validation here

                var result = await categoryService.UpdateAsync(category);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            }).WithName("UpdateCategory");

            group.MapDelete("/{id}", async (Guid id, ICategoryService categoryService) =>
            {
                var result = await categoryService.DeleteAsync(id);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result);
            }).WithName("DeleteCategory");
        }
    }
}
