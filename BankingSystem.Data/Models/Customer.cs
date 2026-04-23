using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Data.Models;

public class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}
