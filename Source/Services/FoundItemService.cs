using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Data;
using Source.Models;

namespace Source.Services
{
    public class FoundItemService
    {
        public static void CreateFoundItem(ApplicationDbContext db)
    {
        Console.WriteLine("--------------------------------------------\nCreate Found Item\n--------------------------------------------");
        Console.Write("Enter the name of the found item: ");
        string name = Console.ReadLine();
        Console.Write("Enter a description of the found item: ");
        string description = Console.ReadLine();
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(description))
        {
            Console.WriteLine("--------------------------------------------\nName and description cannot be empty.\n--------------------------------------------");
            return;
        }
        FoundItem foundItem = new FoundItem
        {
            Name = name,
            Description = description,
            DateFound = DateOnly.FromDateTime(DateTime.Now),
            Status = "Unclaimed" // Default status for new found items
        };
        db.FoundItems.Add(foundItem);
        db.SaveChanges();
        Console.WriteLine("--------------------------------------------\nFound item created successfully.\n--------------------------------------------");
        return;
    }
    }
}