using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using JagiCore.Admin;
using JagiCore.Admin.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MitTraining.Controllers
{
    [Route("api/[Controller]"), ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpGet("Test")]
        public ActionResult<string> Test()
        {
            return Ok("test");
        }

        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody]LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            foreach(var validator in _userManager.PasswordValidators)
            {
                if (await validator.ValidateAsync(_userManager, user, model.Password) != IdentityResult.Success)
                    return BadRequest("Password validation failed");

            }

            return await CreateUserToken(user);
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            var username = principal.Identity.Name;
            var user = await _userManager.FindByNameAsync(username);
            if (user.RefreshToken != refreshToken)
                return BadRequest("Invalid refresh Token");

            return await CreateUserToken(user);
        }

        private async Task<IActionResult> CreateUserToken(ApplicationUser user)
        {
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            var result = GenerateToken(user.Email, user.UserName);
            return new ObjectResult(new { token = result, refreshToken = newRefreshToken });
        }

        private object GenerateToken(string email, string username)
        {
            // Create the token
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Token:Issuer"],
                _configuration["Token:Audience"],
                claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(3),
                signingCredentials: creds);

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            };
        }

        /// <summary>
        /// Generate refresh token
        /// </summary>
        /// <returns></returns>
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// 從舊的 Token 取出登入使用者的資訊
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,   //you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"])),
                // 這裡代表已經失效的 Token 被認為是有效的
                ValidateLifetime = false    //here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid Token");

            return principal;
        }
    }

    public class LoginModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
