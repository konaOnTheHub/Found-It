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

        //Manage found items method
        public static void ManageFoundItems(ApplicationDbContext db)
        {
            Console.WriteLine("\n--------------------------------------------\nManage Found Items\n--------------------------------------------\n");
            //Get all found items from the database
            var foundItems = db.FoundItems.ToList();
            if (foundItems.Count == 0)
            {
                Console.WriteLine("No found items available.");
                return;
            }
            //Print the found items in a table format using reflection
            PrinterService.printFoundItem(foundItems);
            Console.WriteLine("\nSelect an option:\n1. Update Found Item\n2. Delete Found Item\n3. Back to Main Menu\n--------------------------------------------");
            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    //Update found item method
                    break;
                case "2":
                    //Delete found item method
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

        }
    }
}