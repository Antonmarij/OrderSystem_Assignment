using Microsoft.EntityFrameworkCore;
using OrderSystem.Contexts;
using OrderSystem.Entities;
using System.Diagnostics;

namespace OrderSystem.Services;

public class CustomerService
{
    private readonly DataContext _context;

    public CustomerService(DataContext context)
    {
        _context = context;
    }

    public async Task<int> CreateCustomerAsync(CustomerEntity customer)
    {
        try
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer.CustomerId;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return -1;
        }
    }

    public async Task<CustomerEntity> GetCustomerAsync(int customerId)
    {
        try
        {
            var customer = await _context.Customers
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            return customer;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}
