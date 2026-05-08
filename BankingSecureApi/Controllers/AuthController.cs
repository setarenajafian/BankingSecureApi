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
using Microsoft.AspNetCore.RateLimiting;

namespace BankingSecureApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[EnableRateLimiting("api")]
public class AuthController(IConfiguration config, IUserService userService, ILogger logger) : ControllerBase        //request management
{
    private readonly IConfiguration _config = config;
    private readonly IUserService _userService = userService;
    private readonly ILogger _logger = logger;

    //این دیکشنری تعداد تلاش های ناموفق هر کاربر رو نگه میداره
    private static Dictionary<string, int> loginAttempts = new();
    
    [HttpPost("login")]
    public IActionResult Login([FromBody] Models.LoginRequest login)
    {
        //hard code : instance of users for testing 
        var users = new Dictionary<string, (string PasswordHash, string Role)>
        {
            {"Admin" , (_userService.HashPassword("Aa123"),"admin")},
            {"User" , (_userService.HashPassword("Bb123"),"user") }
        };


        if (!users.ContainsKey(login.Username))
        {
            _logger.LogWarning("Login failed for non-existing user {Username}", login.Username);
            return Unauthorized("Invalid username or password");
        }

        //قبل از بررسی رمز چک کنیم کاربر قفل نشده باشد
        if (loginAttempts.ContainsKey(login.Username) && loginAttempts[login.Username] >= 5)
        {
            _logger.LogWarning("User {Username} locked due to too many login attempts", login.Username);
            return StatusCode(423, "Account temporarily locked");
        }


        var user = users[login.Username];
        var passwordValid = _userService.VerifyPassword(login.Password, user.PasswordHash);

        if (!passwordValid)
        {
            if (loginAttempts.ContainsKey(login.Username))
            {
                loginAttempts[login.Username] = 0;
            }
            loginAttempts[login.Username] ++;

            _logger.LogWarning("Invalid password for user {Username}", login.Username);
            return Unauthorized("Invalid username or password");
        }
        loginAttempts.Remove(login.Username);  //اینجا بعد از لاگین موفق پاک می‌شود


        //Implement secure logging for authentication events 
        //check attempts fot loggin
        _logger.LogInformation("login attempt for user : {Username}", login.Username);

        //check unsuccessful login
        if (!users.ContainsKey(login.Username))
        {
            _logger.LogWarning("Invalid login attempt for username: {Username}", login.Username);
            return Unauthorized("Invalid credentials");
        }

        //check successfull login
        _logger.LogInformation("User {Username} logged in successfully", login.Username);





        //authentication flow: check if user exists, validate password and if valid, generate and return a JWT token.
        if (!users.ContainsKey(login.Username))
            return Unauthorized("Invalid credentials");


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
