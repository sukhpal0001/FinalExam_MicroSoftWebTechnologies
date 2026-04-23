using BankingSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Data.Data;

// Shared EF Core context used by both the MVC portal and the ATM API.
public class BankingDbContext(DbContextOptions<BankingDbContext> options) : DbContext(options)
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure unique fields and entity relationships for the banking schema.
        modelBuilder.Entity<Customer>()
            .HasIndex(customer => customer.Email)
            .IsUnique();

        modelBuilder.Entity<Account>()
            .HasIndex(account => account.AccountNumber)
            .IsUnique();

        modelBuilder.Entity<Account>()
            .HasOne(account => account.Customer)
            .WithMany(customer => customer.Accounts)
            .HasForeignKey(account => account.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .HasOne(transaction => transaction.Account)
            .WithMany(account => account.Transactions)
            .HasForeignKey(transaction => transaction.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed starter records so the app has data immediately after migration.
        modelBuilder.Entity<Customer>().HasData(
            new Customer
            {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Carter",
                Email = "john.carter@example.com",
                DateOfBirth = new DateTime(1990, 4, 15)
            },
            new Customer
            {
                CustomerId = 2,
                FirstName = "Maria",
                LastName = "Singh",
                Email = "maria.singh@example.com",
                DateOfBirth = new DateTime(1987, 11, 3)
            });

        modelBuilder.Entity<Account>().HasData(
            new Account
            {
                AccountId = 1,
                AccountNumber = "ACC-10001",
                Balance = 1750.00m,
                AccountType = AccountType.Checking,
                CustomerId = 1
            },
            new Account
            {
                AccountId = 2,
                AccountNumber = "ACC-10002",
                Balance = 4200.00m,
                AccountType = AccountType.Savings,
                CustomerId = 1
            },
            new Account
            {
                AccountId = 3,
                AccountNumber = "ACC-10003",
                Balance = 980.50m,
                AccountType = AccountType.Checking,
                CustomerId = 2
            });

        modelBuilder.Entity<Transaction>().HasData(
            new Transaction
            {
                TransactionId = 1,
                AccountId = 1,
                Amount = 1500.00m,
                TransactionType = TransactionType.Deposit,
                TransactionDate = new DateTime(2026, 4, 1, 9, 0, 0),
                Description = "Initial deposit",
                BalanceAfter = 1500.00m
            },
            new Transaction
            {
                TransactionId = 2,
                AccountId = 1,
                Amount = 250.00m,
                TransactionType = TransactionType.Deposit,
                TransactionDate = new DateTime(2026, 4, 10, 14, 30, 0),
                Description = "Paycheck top-up",
                BalanceAfter = 1750.00m
            },
            new Transaction
            {
                TransactionId = 3,
                AccountId = 2,
                Amount = 4200.00m,
                TransactionType = TransactionType.Deposit,
                TransactionDate = new DateTime(2026, 4, 5, 10, 15, 0),
                Description = "Savings opening balance",
                BalanceAfter = 4200.00m
            },
            new Transaction
            {
                TransactionId = 4,
                AccountId = 3,
                Amount = 1000.00m,
                TransactionType = TransactionType.Deposit,
                TransactionDate = new DateTime(2026, 4, 2, 8, 45, 0),
                Description = "Initial deposit",
                BalanceAfter = 1000.00m
            },
            new Transaction
            {
                TransactionId = 5,
                AccountId = 3,
                Amount = 19.50m,
                TransactionType = TransactionType.Withdrawal,
                TransactionDate = new DateTime(2026, 4, 20, 16, 20, 0),
                Description = "ATM cash withdrawal",
                BalanceAfter = 980.50m
            });
    }
}
