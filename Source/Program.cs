using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Source.Data;
using Source.Models;
using Source.Services;
using System;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Load configuration from appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Get connection string
        string connectionString = config.GetConnectionString("DefaultConnection");

        // Configure EF Core
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        using var db = new ApplicationDbContext(options);
        db.Database.EnsureCreated(); // Ensure DB is created

        User user = null;
        bool running = true;

        while (running)
        {
            if (user != null)
            {
                if (user.Role == "User")
                {
                    Console.WriteLine("\n--- User Menu ---");
                    Console.WriteLine("1. View My Lost Items");
                    Console.WriteLine("2. Report Lost Item");
                    Console.WriteLine("3. View Found Items");
                    Console.WriteLine("4. Logout");
                    Console.Write("Select an option: ");
                    string option = Console.ReadLine();

                    switch (option)
                    {
                        case "1":
                            // call method to view lost item Saki
                            break;

                        case "2":
                            LostItemService.ReportLostItem(db, user); 
                            break;

                        case "3":
                            var foundItems = db.FoundItems.Include(f => f.Claims).ToList();
                            if (foundItems.Count == 0)
                            {
                                Console.WriteLine("No found items available.");
                            }
                            else
                            {
                                PrinterService.printFoundItem(foundItems);
                            }
                            break;

                        case "4":
                            Console.WriteLine("\nLogout successful.");
                            user = null;
                            break;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else if (user.Role == "Admin")
                {
                    Console.WriteLine("\n--- Admin Menu ---");
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
                            FoundItemService.CreateFoundItem(db);
                            break;
                        case "2":
                            FoundItemService.ManageFoundItems(db);
                            break;
                        case "3":
                            // Call method to manage lost items Reyan
                            break;
                        case "4":
                            Console.WriteLine("\nLogout successful.");
                            user = null;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("\n--- Lost & Found System ---");
                Console.WriteLine("1. Login");
                Console.WriteLine("2. Register");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                string option = Console.ReadLine()?.ToLower();

                switch (option)
                {
                    case "1":
                    case "login":
                        user = AuthService.LoginUser(db);
                        if (user != null)
                        {
                            Console.WriteLine($"\nWelcome, {user.Name}!");
                        }
                        else
                        {
                            Console.WriteLine("\nInvalid username or password.");
                        }
                        break;

                    case "2":
                    case "register":
                        AuthService.RegisterUser(db);
                        break;

                    case "3":
                    case "exit":
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
