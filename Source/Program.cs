using Microsoft.EntityFrameworkCore;
using Source.Data;
using Microsoft.Extensions.Configuration;
using Source.Models;

class Program
{
    static void Main(string[] args)
    {
        using var db = new ApplicationDbContext();
        var running = true;
        while (running)
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
                User user = LoginUser(db);
                    if (user != null)
                    {
                        Console.WriteLine($"--------------------------------------------\nWelcome, {user.Name}!\n--------------------------------------------");
                        // Add logic for logged-in users here
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
                    var newUser = new User {
                        Name = newName,
                        Email = newEmail,
                        PasswordHash = newPassword,
                        Role = "User" // Default role for new users
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();
                    Console.WriteLine("--------------------------------------------\nUser registered successfully.\n--------------------------------------------");

    }
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
