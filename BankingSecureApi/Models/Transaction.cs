namespace BankingSecureApi.Models;

public class Transaction
{
    public int Id { get; set; }

    public int AccountId { get; set; }

    public decimal Amount { get; set; }

    public string Type { get; set; } = string.Empty; // Deposit / Withdraw / Transfer

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
