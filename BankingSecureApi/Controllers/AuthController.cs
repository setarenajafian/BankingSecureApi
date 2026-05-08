using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using BankingSecureApi.Models;
using BankingSecureApi.Services;

namespace BankingSecureApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IConfiguration config, IUserService userService) : ControllerBase        //request management
{
    private readonly IConfiguration _config = config;
    private readonly IUserService _userService = userService;

    
    [HttpPost("login")]
    public IActionResult Login([FromBody] Models.LoginRequest login)
    {
        var users = new Dictionary<string, (string PasswordHash, string Role)>
        {
            {"Admin" , (_userService.HashPassword("Aa123"),"admin")},
            {"User" , (_userService.HashPassword("Bb123"),"user") }
        };

        if(!users.ContainsKey(login.Username))
            return Unauthorized("Invalid credentials");

        var user = users[login.Username];

        var PasswoordValid = _userService.VerifyPassword(login.Password, user.PasswordHash);

        if(!PasswoordValid)
            return Unauthorized("Invalid credentials");

        var token = GenerateJwtToken(login.Username, user.Role);

        return Ok(new { token });   

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
