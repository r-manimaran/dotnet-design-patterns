using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Interfaces;
using Products.Api.Models;

namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ILogger<CategoriesController> _logger;

        private ICategoriesRepository _categoryService;
        public CategoriesController(ILogger<CategoriesController> logger,
                                    ICategoriesRepository categoryService)
        {
            _logger = logger;

            _categoryService=categoryService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            await _categoryService.AddAsync(category);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories= await _categoryService.GetAllAsync();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var brand = await _categoryService.GetByIdAsync(id);
            return Ok(brand);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory([FromBody] Category brand)
        {
            await _categoryService.UpdateAsync(brand);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            _categoryService.DeleteAsync(id);
            return Ok();
        }        
    }
}
