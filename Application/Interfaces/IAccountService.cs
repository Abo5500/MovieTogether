using Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<AuthenticationResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<RefreshTokensResponse> RefreshTokensAsync(RefreshTokensRequest request);
        Task<bool> RevokeRefreshTokenAsync(string userId,string? refreshToken = null);
    }
}
