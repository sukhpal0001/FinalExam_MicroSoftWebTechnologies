using BankingSystem.Data.Data;
using BankingSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Data.Repositories;

public class BankRepository(BankingDbContext context) : IBankRepository
{
    public async Task<IReadOnlyList<Account>> GetAccountsAsync()
    {
        return await context.Accounts
            .Include(account => account.Customer)
            .OrderBy(account => account.AccountNumber)
            .ToListAsync();
    }

    public async Task<Account?> GetAccountByIdAsync(int accountId)
    {
        return await context.Accounts
            .Include(account => account.Customer)
            .Include(account => account.Transactions.OrderByDescending(transaction => transaction.TransactionDate))
            .FirstOrDefaultAsync(account => account.AccountId == accountId);
    }

    public async Task<IReadOnlyList<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
    {
        return await context.Transactions
            .Where(transaction => transaction.AccountId == accountId)
            .OrderByDescending(transaction => transaction.TransactionDate)
            .ToListAsync();
    }

    public async Task<(bool Success, string Message, Account? Account)> DepositAsync(int accountId, decimal amount, string? description)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(item => item.AccountId == accountId);

        if (account is null)
        {
            return (false, "Account was not found.", null);
        }

        if (amount <= 0)
        {
            return (false, "Deposit amount must be greater than 0.", account);
        }

        account.Balance += amount;

        context.Transactions.Add(new Transaction
        {
            AccountId = accountId,
            Amount = amount,
            TransactionType = TransactionType.Deposit,
            TransactionDate = DateTime.Now,
            Description = description,
            BalanceAfter = account.Balance
        });

        await context.SaveChangesAsync();
        return (true, "Deposit completed successfully.", account);
    }

    public async Task<(bool Success, string Message, Account? Account)> WithdrawAsync(int accountId, decimal amount, string? description)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(item => item.AccountId == accountId);

        if (account is null)
        {
            return (false, "Account was not found.", null);
        }

        if (amount <= 0)
        {
            return (false, "Withdrawal amount must be greater than 0.", account);
        }

        if (account.Balance < amount)
        {
            return (false, "Insufficient funds for this withdrawal.", account);
        }

        account.Balance -= amount;

        context.Transactions.Add(new Transaction
        {
            AccountId = accountId,
            Amount = amount,
            TransactionType = TransactionType.Withdrawal,
            TransactionDate = DateTime.Now,
            Description = description,
            BalanceAfter = account.Balance
        });

        await context.SaveChangesAsync();
        return (true, "Withdrawal completed successfully.", account);
    }
}
