using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Entities;

public class OrderRowEntity
{
    public int Quantity { get; set; }

    [Required]
    public int ProductId { get; set; }
    public virtual ProductEntity Product { get; set; }

    [Required]
    public int OrderId { get; set; }
    public virtual OrderEntity Order { get; set; }

    public decimal Price { get; set; } 
}
