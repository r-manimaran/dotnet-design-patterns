using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Api.Interfaces;
using Products.Api.Models;
using Products.Api.Repositories;

namespace Products.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly ILogger<BrandsController> _logger;
        private readonly IBrandRespository _brandService;
        public BrandsController(ILogger<BrandsController> logger,
                                  IBrandRespository brandService)
        {
            _logger = logger;
            _brandService = brandService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBrand([FromBody] Brand brand)
        {
            await _brandService.AddAsync(brand);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var brands = await _brandService.GetAllAsync();
            return Ok(brands);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var brand = await _brandService.GetByIdAsync(id);
            return Ok(brand);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBrand([FromBody] Brand brand)
        {
            await _brandService.UpdateAsync(brand);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            _brandService.DeleteAsync(id);
            return Ok();
        }
    }
}
