using System;
using System.Threading;
using JagiCore.Admin.Data;
using JagiCore.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MitTraining.Controllers;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace MitTraining.Tests
{
    public class TestAccountController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Mock<IUserStore<ApplicationUser>> _mockUserStore;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private string _refreshToken = "this is a token";
        private string _token;

        public TestAccountController()
        {
            _mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            var mockPasswordValidator = new Mock<IPasswordValidator<ApplicationUser>>();
            _userManager = MockUserManager.GetTestUserManager(_mockUserStore.Object, mockPasswordValidator.Object);

            _mockUserStore.Setup(s => s.FindByNameAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(
                new ApplicationUser { Email = "test@example.com", UserName = "MyName", RefreshToken = _refreshToken });

            _mockUserStore.Setup(s => s.UpdateAsync(It.IsAny<ApplicationUser>(), CancellationToken.None))
                .Callback((ApplicationUser user, CancellationToken token) =>
                {
                    _refreshToken = user.RefreshToken;
                }).ReturnsAsync(IdentityResult.Success);

            mockPasswordValidator.Setup(p => p.ValidateAsync(_userManager, It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["Token:Key"]).Returns("any key should large than 16 bytes");
        }

        [Fact]
        public async void Test_Get_Token()
        {
            var controller = new AccountController(_userManager, _mockConfiguration.Object);
            var action = await controller.GetToken(new LoginModel { UserName = "Any", Email = "any@example.com" });

            var resultType = Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(action);
            var result = Assert.IsAssignableFrom<object>(resultType.Value);

            Assert.Equal(_refreshToken, result.ToDynamic().refreshToken);
        }

        [Fact]
        public async void Test_Refresh_Token()
        {
            var controller = new AccountController(_userManager, _mockConfiguration.Object);
            var action = await controller.GetToken(new LoginModel { UserName = "Any", Email = "any@example.com" });

            var resultType = Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(action);
            var result = Assert.IsAssignableFrom<object>(resultType.Value);

            var tokenObject = JObject.Parse(JsonConvert.SerializeObject(result));
            var refreshToken = (string)tokenObject["refreshToken"];
            Assert.Equal(_refreshToken, refreshToken);

            string token = (string)tokenObject["token"]["token"];

            controller = new AccountController(_userManager, _mockConfiguration.Object);
            var refreshAction = await controller.RefreshToken(token, refreshToken);

            resultType = Assert.IsType<Microsoft.AspNetCore.Mvc.ObjectResult>(refreshAction);
            result = Assert.IsAssignableFrom<object>(resultType.Value);

            tokenObject = JObject.Parse(JsonConvert.SerializeObject(result));
            Assert.Equal(_refreshToken, (string)tokenObject["refreshToken"]);
            Assert.NotEqual(token, (string)tokenObject["token"]["token"]);
        }
    }
}
