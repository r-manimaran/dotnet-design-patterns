using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using jokes.Api.Services;

namespace jokes.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JokesController : ControllerBase
    {
        private readonly IJokesService _jokesService;

        public JokesController(IJokesService jokesService)
        {
            _jokesService = jokesService;
        }

        [HttpGet("GetJoke")]
        public async Task<IActionResult> GetJoke()
        {
            var response = await _jokesService.GetJokeAsync();
            return Ok(response);
        }
    }
}
