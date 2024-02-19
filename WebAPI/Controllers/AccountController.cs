using Application.DTOs.Account;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody][Required] RegisterRequest request)
        {
            return Ok(await _accountService.RegisterAsync(request));
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody][Required] AuthenticationRequest request)
        {
            return Ok(await _accountService.AuthenticateAsync(request));
        }
        [HttpPost("refresh-tokens")]
        public async Task<IActionResult> RefreshTokensAsync([FromBody][Required] RefreshTokensRequest request)
        {
            return Ok(await _accountService.RefreshTokensAsync(request));
        }
        [HttpDelete("revoke-refresh-token")]
        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody][Required] string userId)
        {
            return Ok(await _accountService.RevokeRefreshTokenAsync(userId));
        }
    }
}
