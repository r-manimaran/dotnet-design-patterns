using ReadThroughCache.Services;

namespace ReadThroughCache.Endpoints
{
    public static class ProductsEndpoints
    {
        public static void MapProductsEndpoints(this IEndpointRouteBuilder builder)
        {
            var api = builder.MapGroup("/api/products").WithTags("Products").WithOpenApi();

            api.MapGet("/{id}", async (int id, IProductService productService) =>
            {
                var product = await productService.GetProductByIdAsync(id);

                if (product == null)
                {
                    return Results.NotFound($"Product with Id {id} not exists");
                }

                return Results.Ok(product);


            }).WithName("GetProduct");
        }
    }
}
