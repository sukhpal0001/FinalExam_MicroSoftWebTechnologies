using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Data.Models;

public class Account
{
    [Key]
    public int AccountId { get; set; }

    [Required]
    [RegularExpression(@"^ACC-\d{5}$", ErrorMessage = "Account number must follow the format ACC-XXXXX.")]
    public string AccountNumber { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative.")]
    public decimal Balance { get; set; }

    [Required]
    public AccountType AccountType { get; set; }

    public int CustomerId { get; set; }

    public Customer? Customer { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
