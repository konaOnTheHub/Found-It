using Microsoft.EntityFrameworkCore;
using Source.Data;
using Microsoft.Extensions.Configuration;
using Source.Models;

class Program
{
    static void Main(string[] args)
    {
        var config = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

        string connectionString = config.GetConnectionString("DefaultConnection");
        using var db = new ApplicationDbContext(connectionString);

        User user = null;

        var running = true;
        while (running)
        {
            if (user != null)
            {
                //Means user has logged in
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
                        user = LoginUser(db);
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
                        RegisterUser(db);

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
    //User Registration method
    static void RegisterUser(ApplicationDbContext db)
    {
        Console.WriteLine("--------------------------------------------\nRegister\n--------------------------------------------");
        Console.Write("Enter your name:");
        string newName = Console.ReadLine();
        Console.Write("Enter your email address: ");
        string newEmail = Console.ReadLine();
        Console.Write("Enter your password: ");
        string newPassword = Console.ReadLine();
        //Validate email format
        if (!newEmail.Contains("@") || !newEmail.Contains("."))
        {
            Console.WriteLine("--------------------------------------------\nInvalid email format. Please try again.\n--------------------------------------------");
            return;
        }
        // Validate password length
        if (newPassword.Length < 6)
        {
            Console.WriteLine("--------------------------------------------\nPassword must be at least 6 characters long. Please try again.\n--------------------------------------------");
            return;
        }
        // Check if the email already exists
        var existingUser = db.Users.FirstOrDefault(u => u.Email == newEmail);
        if (existingUser != null)
        {
            Console.WriteLine("--------------------------------------------\nEmail already exists. Please try again.\n--------------------------------------------");
            return;
        }

        // Create a new user and save to the database
        var newUser = new User
        {
            Name = newName,
            Email = newEmail,
            PasswordHash = newPassword,
            Role = "User" // Default role for new users
        };
        db.Users.Add(newUser);
        db.SaveChanges();
        Console.WriteLine("--------------------------------------------\nUser registered successfully.\n--------------------------------------------");

    }
    //User Login method
    //This method will check if the user exists in the database and return the user object if it does.
    static User LoginUser(ApplicationDbContext db)
    {
        Console.WriteLine("--------------------------------------------\nLogin\n--------------------------------------------");
        Console.Write("Enter your email address: ");
        string email = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();
        var user = db.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        if (user != null)
        {
            return user;
        }
        else
        {
            return null;
        }
    }
}
