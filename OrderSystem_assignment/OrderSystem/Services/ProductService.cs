using OrderSystem.Contexts;
using OrderSystem.Entities;
using System.Diagnostics;

namespace OrderSystem.Services;

public class ProductService
{
    private readonly DataContext _context;

    public ProductService(DataContext context)
    {
        _context = context;
    }

    public List<ProductEntity> GetProducts()
    {
        try
        {
            var products = new List<ProductEntity>
            {
                new ProductEntity { Name = "Car", Description = "Fast, probably.", Price = 800000m },
                new ProductEntity { Name = "Cup", Description = "Nice, probably.", Price = 18m },
                new ProductEntity { Name = "Glasses", Description = "C, probably.", Price = 100m }
            };

            return products;
            
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}

