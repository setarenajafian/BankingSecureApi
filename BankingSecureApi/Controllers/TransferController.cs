using BankingSecureApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSecureApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TransferController : ControllerBase
{
    [HttpPost]
    public IActionResult Transfer([FromBody] TransferRequest request)
    {
        return Ok(new { message = "Transfer request received." });
    }

}
