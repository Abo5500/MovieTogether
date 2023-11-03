using Application.DTOs.Movie;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetByIdAsync([FromQuery]int id)
        {
            return Ok(await _movieService.GetByIdAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetAllAsync(filter));
        }
        [HttpGet("liked")]
        public async Task<IActionResult> GetLikedAsync([FromQuery]string userName, [FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetLikedAsync(userName, filter));
        }
        [Authorize]
        [HttpPatch("setIsLiked")]
        public async Task<IActionResult> SetIsLikedAsync([FromQuery]int movieId, [FromQuery]bool isLiked)
        {
            return Ok(await _movieService.SetIsLiked(movieId, isLiked));
        }
        [HttpGet("byActorId")]
        public async Task<IActionResult> GetByActorIdAsync([FromQuery] int actorId, [FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetByActorIdAsync(actorId, filter));
        }
        [HttpGet("byDirectorId")]
        public async Task<IActionResult> GetByDirectorIdAsync([FromQuery] int directorId, [FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetByDirectorIdAsync(directorId, filter));
        }
        [HttpGet("byTitle")]
        public async Task<IActionResult> GetByTitleAsync([FromQuery] string title, [FromQuery]MovieFilterDTO filter)
        {
            return Ok(await _movieService.GetByTitleAsync(title, filter));
        }
    }
}
