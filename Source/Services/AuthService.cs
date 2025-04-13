using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Data;
using Source.Models;

namespace Source.Services
{
    public static class AuthService
    {
         //User Registration method
    public static void RegisterUser(ApplicationDbContext db)
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
    public static User LoginUser(ApplicationDbContext db)
    {
        Console.WriteLine("--------------------------------------------\nLogin\n--------------------------------------------");
        Console.Write("Enter your email address: ");
        string email = Console.ReadLine();
        Console.Write("Enter your password: ");
        string password = Console.ReadLine();
        //Find the user in the database where email and password match
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
}