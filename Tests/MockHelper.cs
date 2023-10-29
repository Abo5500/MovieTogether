using Application.Enums;
using Domain.Entities.Account;
using Domain.Settings;
using Infrastructure.Contexts;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
        public static Mock<IHttpContextAccessor> GetHttpContextAccessor()
        {
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext());
            return mockHttpContextAccessor;
        }
        public static Mock<IOptions<JwtSettings>> GetJwtSettings()
        {
            var mock = new Mock<IOptions<JwtSettings>>();
            mock.Setup(x => x.Value).Returns(new JwtSettings()
            {
                Audience = "localhost",
                Issuer = "localhost",
                DurationInMinutes = 60,
                Key = "tRfWxrheVFmTOZvG_HeV5U4QWXJ-kOoTcIf8QuMj3VJxvre_6fb_PQBviGGwkgrb"
            });
            return mock;
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
