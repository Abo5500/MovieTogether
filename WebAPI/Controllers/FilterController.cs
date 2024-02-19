using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly IFilterService _filterService;

        public FilterController(IFilterService filterService)
        {
            _filterService = filterService;
        }
        [HttpGet("genres")]
        public async Task<IActionResult> GetGenresAsync()
        {
            return Ok(await _filterService.GetGenresAsync());
        }
        [HttpGet("countries")]
        public async Task<IActionResult> GetCountriesAsync()
        {
            return Ok(await _filterService.GetCountriesAsync());
        }
        [HttpGet("actors")]
        public async Task<IActionResult> GetActorsAsync([FromQuery]int page = 1)
        {
            return Ok(await _filterService.GetActorsAsync(page));
        }
        [HttpGet("directors")]
        public async Task<IActionResult> GetDirectorsAsync([FromQuery]int page = 1)
        {
            return Ok(await _filterService.GetDirectorsAsync(page));
        }
        [HttpGet("actorsByName")]
        public async Task<IActionResult> GetActorsByNameAsync([FromQuery][Required] string name, [FromQuery]int page = 1)
        {
            return Ok(await _filterService.GetActorsByNameAsync(name, page));
        }
        [HttpGet("directorsByName")]
        public async Task<IActionResult> GetDirectorsByNameAsync([FromQuery][Required] string name, [FromQuery]int page = 1)
        {
            return Ok(await _filterService.GetDirectorsByNameAsync(name, page));
        }
        [HttpGet("sorts")]
        public IActionResult GetMovieSorts()
        {
            return Ok(_filterService.GetMovieSorts());
        }
    }
}
