using System.ComponentModel.DataAnnotations;

namespace BankingSecureApi.Models;

public class LoginRequest
{
    [Required]
    [MinLength(3)]
    public string Username { get; set; }

    [Required]
    [MinLength(5)]
    public string Password { get; set; }
}
