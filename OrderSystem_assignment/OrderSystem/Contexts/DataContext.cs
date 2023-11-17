using Microsoft.EntityFrameworkCore;
using OrderSystem.Entities;

namespace OrderSystem.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
    public DbSet<OrderRowEntity> OrderRows { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OrderRowEntity>()
            .HasKey(k => new { k.OrderId, k.ProductId });

        modelBuilder.Entity<OrderRowEntity>()
            .HasOne(k => k.Order)
            .WithMany(k => k.OrderRows)
            .HasForeignKey(k => k.OrderId);

        modelBuilder.Entity<OrderRowEntity>()
            .HasOne(k => k.Product)
            .WithMany()
            .HasForeignKey(k => k.ProductId);


        base.OnModelCreating(modelBuilder);
    }
}
