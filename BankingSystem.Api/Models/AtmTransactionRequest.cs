using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.Models;

public class AtmTransactionRequest
{
    [Required]
    public int AccountId { get; set; }

    [Required]
    [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Amount must be greater than zero")]
    public decimal Amount { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }
}
