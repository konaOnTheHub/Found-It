using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Source.Data;
using Source.Models;

namespace Source.Services
{
    public static class LostItemService
    {// Method to view lost items reported by the current user
public static void ViewLostItems(ApplicationDbContext db, User currentUser)
{
    Console.WriteLine("--------------------------------------------\nView Lost Items\n--------------------------------------------");

    // Retrieve lost items reported by the current user
    var lostItems = db.LostItems.Where(item => item.UserId == currentUser.UserId).ToList();

    if (!lostItems.Any())
    {
        Console.WriteLine("You haven't reported any lost items yet.");
        return;
    }

    // Display the list of lost items
    foreach (var item in lostItems)
    {
        Console.WriteLine($"ID: {item.ItemId} | Name: {item.Name} | Description: {item.Description} | Location: {item.Location} | Date Lost: {item.DateLost} | Status: {item.Status}");
    }

    // Optionally, you can add the functionality to allow the user to select an item for further actions (e.g., update status, claim, etc.)
    Console.WriteLine("\nEnter the ID of the item you want to view or update (or press Enter to go back):");
    string input = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(input)) return;

    if (int.TryParse(input, out int itemId))
    {
        var selectedItem = db.LostItems.FirstOrDefault(item => item.ItemId == itemId);
        if (selectedItem != null)
        {
            Console.WriteLine($"You selected: {selectedItem.Name} - {selectedItem.Description}");
            // Add functionality to view or update the item (e.g., change status, delete, etc.)
        }
        else
        {
            Console.WriteLine("Item not found.");
        }
    }
    else
    {
        Console.WriteLine("Invalid input.");
    }
}

        // Method to report a lost item
        public static void ReportLostItem(ApplicationDbContext db, User currentUser)
        {
            Console.Clear();
            PrinterService.PrintHeader();
            Console.WriteLine("--------------------------------------------\nReport Lost Item\n--------------------------------------------");

            // Gets item details 
            Console.Write("Enter the name of the lost item: ");
            string name = Console.ReadLine();
            Console.Write("Enter a description: ");
            string description = Console.ReadLine();
            Console.Write("Enter the location where it was lost: ");
            string location = Console.ReadLine();

            // Check if all fields are filled
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(location))
            {
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("All fields are required. Please try again.");
                return;
            }

            // Create a new lost item
            var lostItem = new LostItem
            {
                Name = name,
                Description = description,
                Location = location,
                DateLost = DateOnly.FromDateTime(DateTime.Now),
                Status = "Lost",
                UserId = currentUser.UserId
            };

            // Add the lost item to the database and save changes
            db.LostItems.Add(lostItem);
            db.SaveChanges();
            Console.Clear();
            PrinterService.PrintHeader();
            Console.WriteLine("Lost item reported successfully.");
        }

        // Method to manage and update lost items
        public static void ManageLostItems(ApplicationDbContext db)
        {
            Console.Clear();
            PrinterService.PrintHeader();
            Console.WriteLine("--------------------------------------------\nManage Lost Items\n--------------------------------------------");

            // Retrieve all lost items from the database including user details
            var lostItems = db.LostItems.Include(i => i.User).ToList();

            // Check if there are no lost items
            if (!lostItems.Any())
            {
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("No lost items reported yet.");
                return;
            }

            // Display the list of lost items
            foreach (var item in lostItems)
            {
                Console.WriteLine($"ID: {item.ItemId} | Name: {item.Name} | Description: {item.Description} | Location: {item.Location} | Date Lost: {item.DateLost} | Status: {item.Status} | Reported By: {item.User?.Name}");
            }


            Console.WriteLine("\nEnter the ID of the item you want to update (or press Enter to go back):");
            string input = Console.ReadLine();

            // If input is empty or invalid, return
            if (!int.TryParse(input, out int itemId))
            {
                Console.Clear();
                PrinterService.PrintHeader();
                return;
            }

            // Retrieve the selected item
            var selectedItem = db.LostItems.FirstOrDefault(i => i.ItemId == itemId);
            if (selectedItem == null)
            {
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("Item not found.");
                return;
            }

            // Ask for the new status of the selected item
            Console.Write("Update Status (e.g., Found, Claimed, Lost): ");
            string newStatus = Console.ReadLine();

            // If the new status is valid, update it
            if (!string.IsNullOrWhiteSpace(newStatus))
            {
                selectedItem.Status = newStatus;
                db.SaveChanges();
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("Status updated successfully.");
            }
            else
            {
                // Handle invalid status input
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("Invalid status.");
            }
        }
    }
}
