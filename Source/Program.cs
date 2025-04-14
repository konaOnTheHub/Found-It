using Microsoft.EntityFrameworkCore;
using Source.Data;
using Microsoft.Extensions.Configuration;
using Source.Models;
using Source.Services;




class Program
{
    static void Main(string[] args)
    {
        // Load configuration from appsettings.json
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
        // Get the connection string from the configuration
        string connectionString = config.GetConnectionString("DefaultConnection");
        using var db = new ApplicationDbContext(connectionString);
        db.Database.EnsureCreated(); // Ensure the database is created
        //Instantiate the user object to null
        //This will be used to check if the user is logged in or not.
        User user = null;

        var running = true;
        while (running)
        {
            if (user != null)
            {
                if (user.Role == "User")
                {
                    Console.WriteLine("1. View My Lost Items");
                    Console.WriteLine("2. Report Lost Item");
                    Console.WriteLine("3. View Found Items");
                    Console.WriteLine("4. Logout");
                    Console.Write("Select an option: ");
                    string option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            // Call method to view lost items
                            break;
                        case "2":
                            // Call method to report lost item
                            break;
                        case "3":
                            // call method to view found items
                            break;
                        case "4":
                            Console.WriteLine("--------------------------------------------\nLogout successful.\n--------------------------------------------");
                            user = null; // Logout
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else if (user.Role == "Admin")
                {
                    Console.WriteLine("1. Create Found Item");
                    Console.WriteLine("2. Manage Found Items");
                    Console.WriteLine("3. Manage Lost Items");
                    Console.WriteLine("4. Manage Claims");
                    Console.WriteLine("5. Logout");
                    Console.Write("Select an option: ");
                    string option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            //Call method to create found item
                            FoundItemService.CreateFoundItem(db);
                            break;
                        case "2":
                            // Call method to manage found items
                            FoundItemService.ManageFoundItems(db);
                            break;
                        case "3":
                            // Call method to manage lost items
                            break;
                        case "4":
                            // Call method to manage claims
                            break;
                        case "5":
                            user = null; // Logout
                            Console.WriteLine("--------------------------------------------\nLogout successful.\n--------------------------------------------");
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
            else
            //Means user has not logged in
            {
                Console.WriteLine("Generic Bar Name Lost & Found Management System");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                string option = Console.ReadLine();
                switch (option)
                {
                    case "1":
                        user = AuthService.LoginUser(db);
                        if (user != null)
                        {
                            Console.WriteLine($"--------------------------------------------\nWelcome, {user.Name}!\n--------------------------------------------");
                        }
                        else
                        {
                            Console.WriteLine("--------------------------------------------\nInvalid username or password.\n--------------------------------------------");
                        }
                        break;
                    case "2":
                        AuthService.RegisterUser(db);

                        break;
                    case "3":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

    }

}
