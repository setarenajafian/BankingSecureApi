using BankingSecureApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BankingSecureApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableRateLimiting("api")]
public class TransferController : ControllerBase
{
    [HttpPost]
    public IActionResult Transfer([FromBody] TransferRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if(request.FromAccountId == request.ToAccountId)
            return BadRequest(new { message = "Source and destination accounts cannot be the same." });

        if (request.Amount <= 0)
            return BadRequest(new { message = "Transfer amount must be greater than zero." });


        return Ok(new { message = "Transfer request received." });
    }

}
