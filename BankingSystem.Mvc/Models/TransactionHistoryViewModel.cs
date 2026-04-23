using BankingSystem.Data.Models;

namespace BankingSystem.Mvc.Models;

public class TransactionHistoryViewModel
{
    public int AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public IReadOnlyList<Transaction> Transactions { get; set; } = [];
}
