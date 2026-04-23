namespace BankingSystem.Api.Models;

public class InsufficientFundsResponse : ApiErrorResponse
{
    public decimal CurrentBalance { get; set; }
}
