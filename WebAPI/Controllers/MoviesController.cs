using Application.DTOs.Movie;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController: ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet("byId")]
        public async Task<IActionResult> GetByIdAsync([FromQuery][Required] int id)
        {
            return Ok(await _movieService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetAllAsync(filter));
        }
        [HttpGet("liked")]
        public async Task<IActionResult> GetLikedAsync([FromQuery][Required] string userName, [FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetLikedAsync(userName, filter));
        }
        [Authorize]
        [HttpPatch("setIsLiked")]
        public async Task<IActionResult> SetIsLikedAsync([FromQuery][Required]int id, [FromQuery]bool isLiked)
        {
            return Ok(await _movieService.SetIsLiked(id, isLiked));
        }
        [HttpGet("byActorId")]
        public async Task<IActionResult> GetByActorIdAsync([FromQuery][Required] int id, [FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetByActorIdAsync(id, filter));
        }
        [HttpGet("byDirectorId")]
        public async Task<IActionResult> GetByDirectorIdAsync([FromQuery][Required] int id, [FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetByDirectorIdAsync(id, filter));
        }
        [HttpGet("byTitle")]
        public async Task<IActionResult> GetByTitleAsync([FromQuery][Required] string title, [FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetByTitleAsync(title, filter));
        }
        [HttpGet("coincidences")]
        public async Task<IActionResult> GetCoincidencesAsync([FromQuery][Required] List<string> usernames)
        {
            return Ok(await _movieService.GetMovieCoincidencesAsync(usernames));
        }
    }
}
