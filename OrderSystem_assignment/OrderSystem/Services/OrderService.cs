using Microsoft.EntityFrameworkCore;
using OrderSystem.Contexts;
using OrderSystem.Entities;
using System.Diagnostics;

namespace OrderSystem.Services;

public class OrderService
{
    private readonly DataContext _context;
    private readonly ProductService _productService;

    public OrderService(DataContext context, ProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task CreateOrderAsync(CustomerEntity customer, List<(ProductEntity Product, int Quantity)> products)
    {
        try
        {
            var _products = _productService.GetProducts();

            var order = new OrderEntity
            {
                Customer = customer,
                OrderRows = products.Select(p => new OrderRowEntity
                {
                    Product = _productService.GetProducts().FirstOrDefault(pp => pp.Name == p.Product.Name),
                    Quantity = p.Quantity,
                    Price = _productService.GetProducts().FirstOrDefault(pp => pp.Name == p.Product.Name)?.Price ?? 0.0m
                }).ToList()
            };

            order.TotalPrice = order.OrderRows.Sum(or => or.Price * or.Quantity);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        
    }

    public async Task UpdateOrderAsync(int orderId, List<(ProductEntity Product, int Quantity)> products)
    {
        try
        {
            var order = await _context.Orders
                .Include(o => o.OrderRows)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return;
            }

            order.OrderRows.Clear();

            foreach (var productTuple in products)
            {
                var product = _productService.GetProducts().FirstOrDefault(pp => pp.Name == productTuple.Product.Name);


                if (product != null)
                {
                    var orderRow = new OrderRowEntity
                    {
                        Product = product,
                        Quantity = productTuple.Quantity,
                        Price = product.Price
                    };

                    order.OrderRows.Add(orderRow);
                }
            }

            order.TotalPrice = order.OrderRows.Sum(or => or.Price * or.Quantity);

            await _context.SaveChangesAsync();

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public async Task DeleteOrderAsync(int orderId)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);

            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("There is no order with the given ID.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public async Task<List<OrderEntity>> ShowOrdersAsync()
    {
        try
        {
            var orders = await _context.Orders
                .Include(o => o.OrderRows)
                .ThenInclude(or => or.Product)
                .ToListAsync();

            return orders;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<OrderEntity> GetOrderAsync(int orderId)
    {
        try
        {
            var order = await _context.Orders
                .Include(o => o.OrderRows)
                    .ThenInclude(or => or.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            return order;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}
