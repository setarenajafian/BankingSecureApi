using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSecureApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    [HttpGet("account")]
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
