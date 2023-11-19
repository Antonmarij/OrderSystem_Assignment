using OrderSystem.Entities;
using System.Diagnostics;

namespace OrderSystem.Services;

public class MenuService
{
    private readonly CustomerService _customerService;
    private readonly ProductService _productService;
    private readonly OrderService _orderService;

    public MenuService(
        CustomerService customerService,
        ProductService productService,
        OrderService orderService)
    {
        _customerService = customerService;
        _productService = productService;
        _orderService = orderService;
    }

    public async Task ShowAsync()
    {
        var exit = false;

        try
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1. Create Customer");
                Console.WriteLine("2. View Products");
                Console.WriteLine("3. Create an order");
                Console.WriteLine("4. Edit an order");
                Console.WriteLine("5. Delete an order");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");
                var option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        await CreateCustomerAsync();
                        break;

                    case "2":
                        ViewProducts();
                        break;

                    case "3":
                        await CreateOrderAsync();
                        break;

                    case "4":
                        await EditOrderAsync();
                        break;

                    case "5":
                        await DeleteOrderAsync();
                        break;

                    case "0":
                        exit = true;
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Invalid option, please try again!");
                        break;
                }
                Console.ReadKey();
            }
            while (exit == false);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }


    private async Task CreateCustomerAsync()
    {
        try
        {
            var customer = new CustomerEntity();
            Console.Clear();
            Console.WriteLine("Enter Customer Details!");
            Console.WriteLine("-----------------------");

            Console.Write("Enter first name: ");
            customer.FirstName = Console.ReadLine();

            Console.Write("Enter last name: ");
            customer.LastName = Console.ReadLine();

            Console.Write("Enter email: ");
            customer.Email = Console.ReadLine();


            var address = new AddressEntity();
            Console.WriteLine("\nEnter Address Details!");
            Console.WriteLine("-----------------------");

            Console.Write("Enter street: ");
            address.Street = Console.ReadLine();

            Console.Write("Enter postal code: ");
            address.PostalCode = Console.ReadLine();

            Console.Write("Enter city: ");
            address.City = Console.ReadLine();

            customer.Address = address;

            var customerId = await _customerService.CreateCustomerAsync(customer);
            Console.WriteLine($"Customer created with ID: {customerId}");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private void ViewProducts()
    {
        try
        {
            Console.Clear();
            var products = _productService.GetProducts();
            if (products != null)
            {
                Console.WriteLine("Our Products!");
                Console.WriteLine("-------------");

                foreach (var product in products)
                {
                    Console.WriteLine($"\n{product.Name} - {product.Price} SEK\n{product.Description}");
                }

                Console.WriteLine("\nPress Enter to return to the Main menu!");
            }
            else
            {
                Console.WriteLine("Could not retrieve products at this time.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async Task CreateOrderAsync()
    {
        try
        {
            Console.Clear();
            Console.Write("Enter customer ID: ");
            var customerIdInput = Console.ReadLine();
            
            if (int.TryParse(customerIdInput, out int customerId))
            {
                var customer = await _customerService.GetCustomerAsync(customerId);
                if (customer != null)
                {
                    ViewProducts();

                    Console.WriteLine("\nCreate Order:");
                    Console.WriteLine("--------------");
                    Console.Write("Enter product name: ");
                    var productName = Console.ReadLine();

                    Console.Write("Enter quantity: ");
                    var quantityInput = Console.ReadLine();
                    
                    if(int.TryParse(quantityInput, out int quantity))
                    {
                        var products = new List<(ProductEntity Product, int quantity)>
                        {
                            (_productService.GetProducts().FirstOrDefault(p => p.Name == productName), quantity)
                        };

                        await _orderService.CreateOrderAsync(customer, products);
                        Console.WriteLine("Successfully created order!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }
                else
                {
                    Console.WriteLine("Customer not found, try again.");
                }
            }
            else
            {
                Console.WriteLine("Invalid customer ID input.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async Task EditOrderAsync()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Edit Order:");
            Console.WriteLine("-----------");

            //dont have this need to implement ask later.
            await ShowOrdersAsync();

            Console.Write("Enter ID of the order you wish to edit: ");
            var orderIdInput = Console.ReadLine();  

            if (int.TryParse(orderIdInput, out int orderId))
            {
                var order = await _orderService.GetOrderAsync(orderId);

                if (order != null)
                {
                    Console.WriteLine($"Editing Order:{order.OrderId}");
                    Console.WriteLine("-------------------------");

                    foreach (var orderRow in order.OrderRows)
                    {
                        Console.WriteLine($"{orderRow.Quantity}x {orderRow.Product.Name}");
                    }

                    Console.WriteLine("\nEdit order info:");

                    Console.Write("Enter new product name: ");
                    var newProductName = Console.ReadLine();

                    Console.Write("Enter new quantity: ");
                    var newQuantityInput = Console.ReadLine();

                    if (int.TryParse(newQuantityInput, out int newQuantity))
                    {
                        var newProducts = new List<(ProductEntity Products, int Quantity)>
                        {
                            (_productService.GetProducts().FirstOrDefault(p => p.Name == newProductName), newQuantity)
                        };

                        await _orderService.UpdateOrderAsync(orderId, newProducts);
                        Console.WriteLine("Order updated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input.");
                    }
                }
                else
                {
                    Console.WriteLine("Order not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid order ID.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async Task DeleteOrderAsync()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Delete Order:");
            Console.WriteLine("-------------");

            await ShowOrdersAsync();

            Console.Write("Enter order ID of the order you wish to delete: ");
            var orderIdInput = Console.ReadLine();

            if (int.TryParse(orderIdInput, out int orderId))
            {
                await _orderService.DeleteOrderAsync(orderId);
                Console.WriteLine("Order deleted!");
            }
            else
            {
                Console.WriteLine("Order with the given ID does not exist.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async Task ShowOrdersAsync()
    {
        try
        {
            Console.Clear();
            Console.WriteLine("Current Orders:");
            Console.WriteLine("---------------");

            var orders = await _orderService.ShowOrdersAsync();

            if (orders != null && orders.Any())
            {
                foreach (var order in orders)
                {
                    Console.WriteLine($"Order id: {order.OrderId}");
                    Console.WriteLine("--------------------");

                    foreach (var orderRow in order.OrderRows)
                    {
                        Console.WriteLine($"{orderRow.Quantity}x {orderRow.Product.Name}");
                    }
                    Console.WriteLine("-----------------------");
                }
            }
            else
            {
                Console.WriteLine("No orders were found.");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }
}
