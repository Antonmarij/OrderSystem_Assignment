using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Entities;

public class CustomerEntity
{
    [Key]
    public int CustomerId { get; set; }

    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;

    [Required]
    public int AddressId { get; set; }
    public AddressEntity Address { get; set; } = null!;

    public virtual ICollection<OrderEntity> Orders { get; set; }
}
