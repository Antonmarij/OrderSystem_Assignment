using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderSystem.Contexts;
using OrderSystem.Services;
using System.Diagnostics;

namespace OrderSystem;

public class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.AddDbContext<DataContext>(x => x.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Orders\OrderSystem_assignment\OrderSystem\Contexts\order_database.mdf;Integrated Security=True;Connect Timeout=30"));


                services.AddScoped<AddressService>();
                services.AddScoped<CustomerService>();
                services.AddScoped<ProductService>();
                services.AddScoped<OrderService>();
                services.AddScoped<MenuService>();

            })
            .Build();
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            try
            {
                var menuService = services.GetRequiredService<MenuService>();
                await menuService.ShowAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        await host.RunAsync();
    }
}