using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Entities;

public class ProductEntity
{
    [Key]
    public int ProductId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }
}
