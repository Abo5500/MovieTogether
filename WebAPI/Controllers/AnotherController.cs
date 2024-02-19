using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnotherController: ControllerBase
    {
        private readonly IAnotherService _actorFinderService;

        public AnotherController(IAnotherService actorFinderService)
        {
            _actorFinderService = actorFinderService;
        }
        [HttpGet("remove")]
        public async Task<IActionResult> RemoveAllActors()
        {
            return Ok(await _actorFinderService.RemoveAllActors());
        }
        [HttpGet("start")]
        public async Task<IActionResult> StartWork()
        {
            return Ok(await _actorFinderService.StartWork());
        }
        [HttpGet("none")]
        public async Task<IActionResult> RemoveNoneActor()
        {
            return Ok(await _actorFinderService.RemoveNoneActor());
        }
        [HttpGet("renamecharsactors")]
        public async Task<IActionResult> RenameCharsActors()
        {
            return Ok(await _actorFinderService.RenameCharsActors());
        }
        [HttpGet("finddirectors")]
        public async Task<IActionResult> FindDirectors()
        {
            return Ok(await _actorFinderService.FindDirectors());
        }
        [HttpGet("renamecharsDirectors")]
        public async Task<IActionResult> RenameCharsDirectors()
        {
            return Ok(await _actorFinderService.RenameCharsDirectors());
        }
    }
}
