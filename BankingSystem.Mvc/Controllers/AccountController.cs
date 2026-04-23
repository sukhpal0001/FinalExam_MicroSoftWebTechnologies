using BankingSystem.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Mvc.Controllers;

public class AccountController(IBankRepository bankRepository) : Controller
{
    public async Task<IActionResult> Index()
    {
        var accounts = await bankRepository.GetAccountsAsync();
        return View(accounts);
    }

    public async Task<IActionResult> Details(int id)
    {
        var account = await bankRepository.GetAccountByIdAsync(id);

        if (account is null)
        {
            return NotFound();
        }

        return View(account);
    }
}
