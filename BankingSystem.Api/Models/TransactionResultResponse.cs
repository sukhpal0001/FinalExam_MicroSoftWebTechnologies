namespace BankingSystem.Api.Models;

public class TransactionResultResponse
{
    public string Message { get; set; } = string.Empty;
    public decimal NewBalance { get; set; }
}
