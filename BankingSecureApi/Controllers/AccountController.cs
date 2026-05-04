using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSecureApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController : ControllerBase
{
    [HttpGet("balance")]
    public IActionResult GetBalance()
    {
        return Ok(new { balance = 1500 });
    }
    [HttpGet("transactions")]
    public IActionResult GetTransactions()
    {
        return Ok(new[] { "Sample transaction 1", "Sample transaction 2" });
    }
}
