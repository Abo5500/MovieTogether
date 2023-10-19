using Application.Enums;
using Domain.Entities.Account;
using Domain.Settings;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class MockHelper
    {
        public static Mock<UserManager<ApplicationUser>> GetUserManager()
        {
            var mockUserManager = new Mock<UserManager<ApplicationUser>>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            mockUserManager.Setup(x => x.GetRolesAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<string>() { Roles.User.ToString() });
            return mockUserManager;
        }
        public static JwtSettings GetJwtSettings()
        {
            return new JwtSettings()
            {
                Audience = "Test",
                Issuer = "Test",
                DurationInMinutes = 60,
                Key = "SuperSecretKeyForSuperSecretTestsInSuperSecretApp"
            };
        }
        public static ApplicationUser GetTestUser()
        {
            return new ApplicationUser()
            {
                Email = "TestEmail",
                UserName = "TestName",
                Id = "TestId"
            };
        }
    }
}
