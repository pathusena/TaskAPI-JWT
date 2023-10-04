using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementAPI.Authentications;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthenticate _authenticateService;
        private readonly IConfiguration _configuration;
        public AuthController(IAuthenticate authenticateService, IConfiguration configuration)
        {
            _authenticateService = authenticateService;
            _configuration = configuration;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserCredentials credentials)
        {
            if (_authenticateService.IsUserAuthenticated(credentials))
            {
                var userId = _authenticateService.GetUserIdFromCredentials(credentials);
                var username = credentials.Username;

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, username)
                };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(issuer: _configuration["JwtSettings:Issuer"], audience: _configuration["JwtSettings:Audience"], claims: claims, expires: DateTime.Now.AddMinutes(6), signingCredentials: signinCredentials);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new JWTTokenResponse
                {
                    Token = tokenString
                });
            }
            else
            {
                return Unauthorized("Invalid credentials");
            }
        }
    }
}
