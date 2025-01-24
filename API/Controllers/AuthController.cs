using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTOs;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginDto login)
        {
            // Aquí deberías validar las credenciales contra tu base de datos
            if (login.Username != "admin" || login.Password != "admin") // Solo para demo
                return Unauthorized();

            var token = GenerateJwtToken(login.Username);
            return Ok(new { token });
        }

        private string GenerateJwtToken(string username)
        {
            var key = _configuration["Jwt:Key"] ??
                throw new InvalidOperationException("JWT Key not found");
            var issuer = _configuration["Jwt:Issuer"] ??
                throw new InvalidOperationException("JWT Issuer not found");
            var audience = _configuration["Jwt:Audience"] ??
                throw new InvalidOperationException("JWT Audience not found");
            var duration = _configuration["Jwt:DurationInMinutes"] ?? "60";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: new[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(duration)),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}