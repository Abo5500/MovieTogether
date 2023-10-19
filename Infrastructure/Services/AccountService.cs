using Application.DTOs.Account;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities.Account;
using Domain.Settings;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        public AccountService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(request.Email)
                ?? throw new ApiException($"Нет аккаунтов, зарегистрированных на {request.Email}.");

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                throw new ApiException($"Неверный email или пароль.");
            }
            AuthenticationResponse response = new AuthenticationResponse();
            response.UserId = user.Id;
            response.AccessToken = await GenerateAccessTokenAsync(user);
            response.Email = user.Email;
            response.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user);
            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.RefreshToken = await GenerateRefreshTokenAsync(user);
            return response;
        }

        public async Task<AuthenticationResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            if (await _userManager.FindByEmailAsync(registerRequest.Email) != null)
                throw new ApiException($"Аккаунт с {registerRequest.Email} уже существует, попробуйте другой");
            if (await _userManager.FindByNameAsync(registerRequest.UserName) != null)
                throw new ApiException($"Логин {registerRequest.UserName} уже занят, попробуйте другой");

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerRequest.UserName,
                Email = registerRequest.Email,
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.User.ToString());
                //TODO: Email verification
                return new AuthenticationResponse()
                {
                    AccessToken = await GenerateAccessTokenAsync(user),
                    RefreshToken = await GenerateRefreshTokenAsync(user),
                    Email = registerRequest.Email,
                    Roles = new List<string> { Roles.User.ToString() },
                    UserName = registerRequest.UserName,
                    UserId = user.Id
                };
            }
            else
            {
                throw new AccountException($"{result.Errors}");
            }
        }

        public async Task<RefreshTokensResponse> RefreshTokensAsync(RefreshTokensRequest request)
        {
            var claims = GetPrincipalFromExpiredToken(request.AccessToken);
            var userId = _userManager.GetUserId(claims) ?? throw new AccountException("Token does not have userId");
            var user = await _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(x => x.Id == userId);
            if (user.RefreshTokens is null)
            {
                throw new ApiException("Пожалуйста, войдите в систему заново");
            }
            if (user.RefreshTokens.Count > 10)
            {
                user.RefreshTokens.Clear();
                await _userManager.UpdateAsync(user);
                throw new ApiException("Пожалуйста, войдите в систему заново");
            }
            var tokenToDelete = user.RefreshTokens.FirstOrDefault(x => x.Token == request.RefreshToken)
                ?? throw new ApiException("Пожалуйста, войдите в систему заново");
            await RevokeRefreshTokenAsync(userId, request.RefreshToken);
            string refreshToken = await GenerateRefreshTokenAsync(user);
            return new RefreshTokensResponse()
            {
                RefreshToken = refreshToken,
                AccessToken = await GenerateAccessTokenAsync(user)
            };
        }
        private async Task<string> GenerateRefreshTokenAsync(ApplicationUser user)
        {
            RefreshToken refreshToken = new RefreshToken() 
            {
                UserId = user.Id,
                Token = GenerateRefreshTokenString(),
                ExpiresAt = DateTime.UtcNow.AddDays(60)
            };
            if (user.RefreshTokens is null)
                user.RefreshTokens = new List<RefreshToken>();
            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);
            return refreshToken.Token;
        }
        public async Task RevokeRefreshTokenAsync(string userId, string refreshToken)
        {
            var user = await _userManager.Users.Include(x => x.RefreshTokens).SingleOrDefaultAsync(x => x.Id == userId)
                ?? throw new AccountException("Invalid userId");
            if (user.RefreshTokens is null)
                return;
            var tokenToRemove = user.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);
            if(tokenToRemove is null)
                return;
            user.RefreshTokens.Remove(tokenToRemove);
            await _userManager.UpdateAsync(user);
        }
        private async Task<string> GenerateAccessTokenAsync(ApplicationUser user)
        {
            //var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            //.Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key)),
                ValidateLifetime = false,
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new AccountException("Invalid Access token");
            return principal;
        }
        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
