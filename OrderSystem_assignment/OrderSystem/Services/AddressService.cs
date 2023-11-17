using OrderSystem.Contexts;
using OrderSystem.Entities;
using System.Diagnostics;

namespace OrderSystem.Services;

public class AddressService
{
    private readonly DataContext _context;

    public AddressService(DataContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAddressAsync(AddressEntity address)
    {
        try
        {
            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();
            return address.AddressId;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return -1;
        }
    }

    public async Task<AddressEntity> GetAddressAsync(int addressId)
    {
        try
        {
            var address = await _context.Addresses.FindAsync(addressId);
            return address;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return null;
        }
    }
}
