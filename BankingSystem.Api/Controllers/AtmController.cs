using BankingSystem.Api.Models;
using BankingSystem.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers;

[ApiController]
[Route("api/atm")]
public class AtmController(IBankRepository bankRepository) : ControllerBase
{
    /// <summary>
    /// Returns the current balance for a specific account.
    /// </summary>
    [HttpGet("balance/{accountId:int}")]
    [ProducesResponseType(typeof(BalanceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBalance(int accountId)
    {
        var account = await bankRepository.GetAccountByIdAsync(accountId);

        if (account is null)
        {
            return NotFound(new ApiErrorResponse { Error = "Account not found" });
        }

        return Ok(new BalanceResponse
        {
            AccountId = account.AccountId,
            AccountNumber = account.AccountNumber,
            Balance = account.Balance
        });
    }

    /// <summary>
    /// Processes an ATM deposit for an existing account.
    /// </summary>
    [HttpPost("deposit")]
    [ProducesResponseType(typeof(TransactionResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deposit([FromBody] AtmTransactionRequest request)
    {
        var account = await bankRepository.GetAccountByIdAsync(request.AccountId);

        if (account is null)
        {
            return NotFound(new ApiErrorResponse { Error = "Account not found" });
        }

        if (request.Amount <= 0)
        {
            return BadRequest(new ApiErrorResponse { Error = "Amount must be greater than zero" });
        }

        var result = await bankRepository.DepositAsync(request.AccountId, request.Amount, request.Description);

        if (!result.Success || result.Account is null)
        {
            return BadRequest(new ApiErrorResponse { Error = "Amount must be greater than zero" });
        }

        return Ok(new TransactionResultResponse
        {
            Message = "Deposit successful",
            NewBalance = result.Account.Balance
        });
    }

    /// <summary>
    /// Processes an ATM withdrawal for an existing account.
    /// </summary>
    [HttpPost("withdraw")]
    [ProducesResponseType(typeof(TransactionResultResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(InsufficientFundsResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Withdraw([FromBody] AtmTransactionRequest request)
    {
        var account = await bankRepository.GetAccountByIdAsync(request.AccountId);

        if (account is null)
        {
            return NotFound(new ApiErrorResponse { Error = "Account not found" });
        }

        if (request.Amount <= 0)
        {
            return BadRequest(new ApiErrorResponse { Error = "Amount must be greater than zero" });
        }

        if (account.Balance < request.Amount)
        {
            return BadRequest(new InsufficientFundsResponse
            {
                Error = "Insufficient funds",
                CurrentBalance = account.Balance
            });
        }

        var result = await bankRepository.WithdrawAsync(request.AccountId, request.Amount, request.Description);

        if (!result.Success || result.Account is null)
        {
            return BadRequest(new InsufficientFundsResponse
            {
                Error = "Insufficient funds",
                CurrentBalance = account.Balance
            });
        }

        return Ok(new TransactionResultResponse
        {
            Message = "Withdrawal successful",
            NewBalance = result.Account.Balance
        });
    }

    /// <summary>
    /// Returns the transaction history for a specific account.
    /// </summary>
    [HttpGet("transactions/{accountId:int}")]
    [ProducesResponseType(typeof(IEnumerable<TransactionHistoryItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetTransactions(int accountId)
    {
        var account = await bankRepository.GetAccountByIdAsync(accountId);

        if (account is null)
        {
            return NotFound(new ApiErrorResponse { Error = "Account not found" });
        }

        var transactions = await bankRepository.GetTransactionsByAccountIdAsync(accountId);

        var response = transactions.Select(transaction => new TransactionHistoryItemResponse
        {
            TransactionId = transaction.TransactionId,
            Amount = transaction.Amount,
            TransactionType = transaction.TransactionType.ToString(),
            TransactionDate = transaction.TransactionDate,
            Description = transaction.Description,
            BalanceAfter = transaction.BalanceAfter
        });

        return Ok(response);
    }
}
