using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using BankingSecureApi.Models;

namespace BankingSecureApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Models.LoginRequest login)
    {
        if (login.Username == "admin" && login.Password == "Aa123")
        {
            var token = GenerateJwtToken("admin","Admin");

            return Ok(new
            {
                token = token
            });
        }
        if ( login.Username == "setare" && login.Password == "Aa123")
        {
            var token = GenerateJwtToken("setare", "User");

            return Ok(new
            {
                token = token
            });
        }

        return Unauthorized();
        
    }


    private string GenerateJwtToken(string username , string role)
    {
        var jwtSettings = _config.GetSection("jwt");

        var Key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]));


        var credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)

        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires : DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["ExpireMinutes"])),
            signingCredentials: credentials
        );


        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
