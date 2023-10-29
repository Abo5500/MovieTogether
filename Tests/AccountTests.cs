using Application.DTOs.Account;
using Application.Enums;
using Application.Exceptions;
using Domain.Settings;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class AccountTests
    {
        [Fact]
        public async Task RegisterSuccess()
        {
            //Arrange
            var registerRequest = new RegisterRequest()
            {
                UserName = "TestName",
                Email = "TestEmail",
                Password = "TestPassword",
                ConfirmPassword = "TestPassword"
            };
            AuthenticationResponse authResponse = new AuthenticationResponse()
            {
                AccessToken = "NotNullOrEmpty",
                Email = "TestEmail",
                UserName = "TestName",
                UserId = "NotNullOrEmpty",
                Roles = new List<string>() { Roles.User.ToString() }
            };
            var mockUserManager = MockHelper.GetUserManager();
            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), registerRequest.Password)).ReturnsAsync(IdentityResult.Success);
            var mockHttpContextAccessor = MockHelper.GetHttpContextAccessor();
            var accountService = new AccountService(mockUserManager.Object, MockHelper.GetJwtSettings().Object, mockHttpContextAccessor.Object);
            //Act
            var result = await accountService.RegisterAsync(registerRequest);
            //Assert
            Assert.Equal(authResponse.Email, result.Email);
            Assert.Equal(authResponse.UserName, result.UserName);
            Assert.Equal(authResponse.Roles.First(), result.Roles.First());
            Assert.False(string.IsNullOrEmpty(result.UserId));
            Assert.NotNull(result.AccessToken);
        }
        [Fact]
        public async Task RegisterEmailAlreadyExists()
        {
            //Arrange
            var registerRequest = new RegisterRequest()
            {
                UserName = "TestFailedName",
                Email = "TestFailedEmail",
                Password = "TestPassword",
                ConfirmPassword = "TestPassword"
            };
            var mockUserManager = MockHelper.GetUserManager();
            mockUserManager.Setup(x => x.FindByEmailAsync(registerRequest.Email)).ReturnsAsync(new ApplicationUser());
            var mockHttpContextAccessor = MockHelper.GetHttpContextAccessor();
            var accountService = new AccountService(mockUserManager.Object, MockHelper.GetJwtSettings().Object, mockHttpContextAccessor.Object);
            //Act
            //Assert
            await Assert.ThrowsAsync<ApiException>(() => accountService.RegisterAsync(registerRequest));
        }
        [Fact]
        public async Task RegisterUserNameAlreadyExists()
        {
            //Arrange
            var registerRequest = new RegisterRequest()
            {
                UserName = "TestFailedName",
                Email = "TestFailedEmail",
                Password = "TestPassword",
                ConfirmPassword = "TestPassword"
            };
            var mockUserManager = MockHelper.GetUserManager();
            mockUserManager.Setup(x => x.FindByNameAsync(registerRequest.UserName)).ReturnsAsync(new ApplicationUser());
            var mockHttpContextAccessor = MockHelper.GetHttpContextAccessor();
            var accountService = new AccountService(mockUserManager.Object, MockHelper.GetJwtSettings().Object, mockHttpContextAccessor.Object);
            //Act
            //Assert
            await Assert.ThrowsAsync<ApiException>(() => accountService.RegisterAsync(registerRequest));
        }
        [Fact]
        public async Task AuthenticateSuccess()
        {
            //Arrange
            ApplicationUser user = MockHelper.GetTestUser();
            var authRequest = new AuthenticationRequest()
            {
                Email = user.Email,
                Password = "TestPassword"
            };
            AuthenticationResponse authResponse = new AuthenticationResponse()
            {
                AccessToken = "NotNullOrEmpty",
                Email = "TestEmail",
                UserName = "TestName",
                UserId = "NotNullOrEmpty",
                Roles = new List<string>() { Roles.User.ToString() }
            };
            var mockUserManager = MockHelper.GetUserManager();

            mockUserManager.Setup(x => x.FindByEmailAsync(authRequest.Email)).ReturnsAsync(user);
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, authRequest.Password)).ReturnsAsync(true);
            var mockHttpContextAccessor = MockHelper.GetHttpContextAccessor();
            var accountService = new AccountService(mockUserManager.Object, MockHelper.GetJwtSettings().Object, mockHttpContextAccessor.Object);
            //Act
            var result = await accountService.AuthenticateAsync(authRequest);
            //Assert
            Assert.Equal(authResponse.Email, result.Email);
            Assert.Equal(authResponse.UserName, result.UserName);
            Assert.Equal(authResponse.Roles.First(), result.Roles.First());
            Assert.False(string.IsNullOrEmpty(result.UserId));
            Assert.NotNull(result.AccessToken);
        }
    }
}
