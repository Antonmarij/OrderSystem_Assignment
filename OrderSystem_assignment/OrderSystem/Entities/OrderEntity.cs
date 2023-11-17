using System.ComponentModel.DataAnnotations;

namespace OrderSystem.Entities;

public class OrderEntity
{
    [Key]
    public int OrderId { get; set; }

    public int CustomerId { get; set; }
    public virtual CustomerEntity Customer { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime OrderDate { get; set; }

    public virtual ICollection<OrderRowEntity> OrderRows { get; set; }
}
