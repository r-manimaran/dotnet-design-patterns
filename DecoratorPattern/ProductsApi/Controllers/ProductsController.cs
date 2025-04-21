using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Services;

namespace ProductsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("{Id:int}")]
        public async Task<IActionResult> GetProduct(int Id)
        {
            var result = await _productService.GetProductById(Id);
            return Ok(result);
        }
    }
}
