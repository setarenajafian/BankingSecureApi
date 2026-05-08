using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BankingSecureApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin")]
[EnableRateLimiting("api")]
public class AdminController : ControllerBase
{
    [HttpGet("dashboard")]
    public IActionResult Dashboard()
    {
        return Ok("Welcome Admin");
    }

    [HttpGet("accounts")]
    public IActionResult GetAllAccounts()
    {
        return Ok(new[] { "Account 1", "Account 2" });
    }

    [HttpGet("transactions")]
    public IActionResult GetAllTransactions()
    { 
        return Ok(new[] { "Tx 1", "Tx 2" });
    }
}
