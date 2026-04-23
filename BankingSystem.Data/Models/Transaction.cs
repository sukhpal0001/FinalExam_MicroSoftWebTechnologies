using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Data.Models;

public class Transaction
{
    [Key]
    public int TransactionId { get; set; }

    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }

    [Required]
    public TransactionType TransactionType { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.Now;

    public int AccountId { get; set; }

    public Account? Account { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Balance after cannot be negative.")]
    public decimal BalanceAfter { get; set; }
}
