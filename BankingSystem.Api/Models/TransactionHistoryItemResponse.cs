namespace BankingSystem.Api.Models;

public class TransactionHistoryItemResponse
{
    public int TransactionId { get; set; }
    public decimal Amount { get; set; }
    public string TransactionType { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string? Description { get; set; }
    public decimal BalanceAfter { get; set; }
}
