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
    
    // Get and validate the username
    string newName;
    do
    {
        Console.Write("Enter your name (no spaces, max 16 characters, cannot be empty): ");
        newName = Console.ReadLine();
    } while (string.IsNullOrWhiteSpace(newName) || newName.Contains(" ") || newName.Length > 16); // Check for empty, spaces, and max length
    
 // Get and validate the email
    string newEmail;
    do
    {
        Console.Write("Enter your email address (max 24 characters): ");
        newEmail = Console.ReadLine();
    } while (newEmail.Length > 24 || newEmail.Length < 5 || !newEmail.Contains("@") || !newEmail.Contains(".")); // Validate length and format
    
    // Validate email length: between 5 and 24 characters
    if (newEmail.Length > 24 || newEmail.Length < 5)
    {
        Console.WriteLine("--------------------------------------------\nEmail must be between 5 and 24 characters. Please try again.\n--------------------------------------------");
        return;
    }

    // Get and validate the password
    string newPassword;
    do
    {
        Console.Write("Enter your password (6-16 characters): ");
        newPassword = Console.ReadLine();
    } while (newPassword.Length > 16); // Validate password length

    // Check if the password is at least 6 characters long
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
        PasswordHash = newPassword, // You might want to hash the password before saving it in a production app
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