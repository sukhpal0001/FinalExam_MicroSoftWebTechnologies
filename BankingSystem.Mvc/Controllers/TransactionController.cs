using BankingSystem.Data.Models;
using BankingSystem.Data.Repositories;
using BankingSystem.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Mvc.Controllers;

public class TransactionController(IBankRepository bankRepository) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Deposit(int accountId)
    {
        var account = await bankRepository.GetAccountByIdAsync(accountId);

        if (account is null)
        {
            TempData["ErrorMessage"] = "Account not found.";
            return RedirectToAction("Index", "Account");
        }

        return View(BuildFormModel(account));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Deposit(TransactionFormViewModel model)
    {
        var account = await bankRepository.GetAccountByIdAsync(model.AccountId);

        if (account is null)
        {
            TempData["ErrorMessage"] = "Account not found.";
            return RedirectToAction("Index", "Account");
        }

        if (!ModelState.IsValid)
        {
            model.AccountNumber = account.AccountNumber;
            model.CurrentBalance = account.Balance;
            return View(model);
        }

        var result = await bankRepository.DepositAsync(model.AccountId, model.Amount, model.Description);
        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

        return RedirectToAction("Details", "Account", new { id = model.AccountId });
    }

    [HttpGet]
    public async Task<IActionResult> Withdraw(int accountId)
    {
        var account = await bankRepository.GetAccountByIdAsync(accountId);

        if (account is null)
        {
            TempData["ErrorMessage"] = "Account not found.";
            return RedirectToAction("Index", "Account");
        }

        return View(BuildFormModel(account));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Withdraw(TransactionFormViewModel model)
    {
        var account = await bankRepository.GetAccountByIdAsync(model.AccountId);

        if (account is null)
        {
            TempData["ErrorMessage"] = "Account not found.";
            return RedirectToAction("Index", "Account");
        }

        if (!ModelState.IsValid)
        {
            model.AccountNumber = account.AccountNumber;
            model.CurrentBalance = account.Balance;
            return View(model);
        }

        var result = await bankRepository.WithdrawAsync(model.AccountId, model.Amount, model.Description);
        TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] = result.Message;

        return RedirectToAction("Details", "Account", new { id = model.AccountId });
    }

    [HttpGet]
    public async Task<IActionResult> History(int accountId)
    {
        var account = await bankRepository.GetAccountByIdAsync(accountId);

        if (account is null)
        {
            TempData["ErrorMessage"] = "Account not found.";
            return RedirectToAction("Index", "Account");
        }

        var transactions = await bankRepository.GetTransactionsByAccountIdAsync(accountId);

        return View(new TransactionHistoryViewModel
        {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            CurrentBalance = account.Balance,
            Transactions = transactions
        });
    }

    private static TransactionFormViewModel BuildFormModel(Account account)
    {
        return new TransactionFormViewModel
        {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            CurrentBalance = account.Balance
        };
    }
}
