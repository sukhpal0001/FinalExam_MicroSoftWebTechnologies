using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Mvc.Models;

public class TransactionFormViewModel
{
    public int AccountId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }

    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }
}
