using BankingSystem.Data.Models;

namespace BankingSystem.Data.Repositories;

public interface IBankRepository
{
    Task<IReadOnlyList<Account>> GetAccountsAsync();
    Task<Account?> GetAccountByIdAsync(int accountId);
    Task<IReadOnlyList<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
    Task<(bool Success, string Message, Account? Account)> DepositAsync(int accountId, decimal amount, string? description);
    Task<(bool Success, string Message, Account? Account)> WithdrawAsync(int accountId, decimal amount, string? description);
}
