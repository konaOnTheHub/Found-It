using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Source.Data;
using Source.Models;

namespace Source.Services
{
    public static class LostItemService
    {
        // Helper method to clear console and print header
        private static void ClearAndPrintHeader(string title)
        {
            Console.Clear();
            PrinterService.PrintHeader();
            Console.WriteLine($"--------------------------------------------\n{title}\n--------------------------------------------");
        }

        // Helper method to print a lost item
        private static void PrintLostItem(LostItem item)
        {
            Console.WriteLine($"ID: {item.ItemId} | Name: {item.Name} | Description: {item.Description} | Location: {item.Location} | Date Lost: {item.DateLost} | Status: {item.Status}");
        }

        // Method to view lost items reported by the current user
      public static void ViewLostItems(ApplicationDbContext db, User currentUser)
{
    while (true)
    {
        ClearAndPrintHeader("View Lost Items");

        Console.WriteLine("1. View All My Lost Items");
        Console.WriteLine("2. Search/Filter My Lost Items");
        Console.WriteLine("3. Back");
        Console.Write("Choose an option: ");
        string choice = Console.ReadLine();

        List<LostItem> lostItems;

        if (choice == "1")
        {
            lostItems = db.LostItems.Where(item => item.UserId == currentUser.UserId).ToList();
        }
        else if (choice == "2")
        {
            Console.Write("Search by Name (leave blank to skip): ");
            string nameFilter = Console.ReadLine()?.ToLower();

            Console.Write("Search by Location (leave blank to skip): ");
            string locationFilter = Console.ReadLine()?.ToLower();

            Console.Write("Search by Status (e.g. Lost, Found) (leave blank to skip): ");
            string statusFilter = Console.ReadLine()?.ToLower();

            lostItems = db.LostItems
                .Where(item => item.UserId == currentUser.UserId &&
                              (string.IsNullOrEmpty(nameFilter) || item.Name.ToLower().Contains(nameFilter)) &&
                              (string.IsNullOrEmpty(locationFilter) || item.Location.ToLower().Contains(locationFilter)) &&
                              (string.IsNullOrEmpty(statusFilter) || item.Status.ToLower().Contains(statusFilter)))
                .ToList();
        }
        else
        {
            return;
        }

        if (!lostItems.Any())
        {
            Console.WriteLine("No matching lost items found.");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            continue;
        }

        foreach (var item in lostItems)
        {
            PrintLostItem(item);
        }

        Console.WriteLine("\nEnter the ID of the item you want to edit/delete (or press Enter to go back):");
        string input = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(input)) continue;

        if (int.TryParse(input, out int itemId))
        {
            var selectedItem = lostItems.FirstOrDefault(item => item.ItemId == itemId);
            if (selectedItem == null)
            {
                Console.WriteLine("Item not found.");
                Console.ReadKey();
                continue;
            }

            Console.WriteLine($"\nSelected Item: {selectedItem.Name} - {selectedItem.Description}");
            Console.WriteLine("1. Edit Item");
            Console.WriteLine("2. Delete Item");
            Console.WriteLine("3. Cancel");
            Console.Write("Choose an option: ");
            var action = Console.ReadLine();

            switch (action)
            {
                case "1": // Edit
                    Console.Write("Enter new name (leave blank to keep current): ");
                    string newName = Console.ReadLine();
                    Console.Write("Enter new description (leave blank to keep current): ");
                    string newDesc = Console.ReadLine();
                    Console.Write("Enter new location (leave blank to keep current): ");
                    string newLocation = Console.ReadLine();
                    Console.Write("Enter new status (leave blank to keep current): ");
                    string newStatus = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newName)) selectedItem.Name = newName;
                    if (!string.IsNullOrWhiteSpace(newDesc)) selectedItem.Description = newDesc;
                    if (!string.IsNullOrWhiteSpace(newLocation)) selectedItem.Location = newLocation;
                    if (!string.IsNullOrWhiteSpace(newStatus)) selectedItem.Status = newStatus;

                    db.SaveChanges();
                    Console.WriteLine("Item updated successfully.");
                    break;

                case "2": // Delete
                    Console.Write("Are you sure you want to delete this item? (yes/no): ");
                    string confirm = Console.ReadLine()?.ToLower();
                    if (confirm == "yes" || confirm == "y")
                    {
                        db.LostItems.Remove(selectedItem);
                        db.SaveChanges();
                        Console.WriteLine("Item deleted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Deletion cancelled.");
                    }
                    break;

                case "3":
                    Console.WriteLine("Action cancelled.");
                    break;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input.");
        }

        Console.WriteLine("Press any key to return to menu...");
        Console.ReadKey();
    }
}


        // Method to report a lost item
        public static void ReportLostItem(ApplicationDbContext db, User currentUser)
        {
            ClearAndPrintHeader("Report Lost Item");

            Console.Write("Enter the name of the lost item: ");
            string name = Console.ReadLine();
            Console.Write("Enter a description: ");
            string description = Console.ReadLine();
            Console.Write("Enter the location where it was lost: ");
            string location = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(location))
            {
                ClearAndPrintHeader("Report Lost Item");
                Console.WriteLine("All fields are required. Please try again.");
                return;
            }

            var lostItem = new LostItem
            {
                Name = name,
                Description = description,
                Location = location,
                DateLost = DateOnly.FromDateTime(DateTime.Now),
                Status = "Lost",
                UserId = currentUser.UserId
            };

            db.LostItems.Add(lostItem);
            db.SaveChanges();

            ClearAndPrintHeader("Report Lost Item");
            Console.WriteLine("Lost item reported successfully.");
        }

        // Method to manage and update all lost items (admin)
        public static void ManageLostItems(ApplicationDbContext db)
        {
            ClearAndPrintHeader("Manage Lost Items");

            var lostItems = db.LostItems.Include(i => i.User).ToList();

            if (!lostItems.Any())
            {
                ClearAndPrintHeader("Manage Lost Items");
                Console.WriteLine("No lost items reported yet.");
                return;
            }

            foreach (var item in lostItems)
            {
                Console.WriteLine($"ID: {item.ItemId} | Name: {item.Name} | Description: {item.Description} | Location: {item.Location} | Date Lost: {item.DateLost} | Status: {item.Status} | Reported By: {item.User?.Name}");
            }

            Console.WriteLine("\nEnter the ID of the item you want to update (or press Enter to go back):");
            string input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input)) return;

            if (!int.TryParse(input, out int itemId))
            {
                ClearAndPrintHeader("Manage Lost Items");
                return;
            }

            var selectedItem = lostItems.FirstOrDefault(i => i.ItemId == itemId);
            if (selectedItem == null)
            {
                ClearAndPrintHeader("Manage Lost Items");
                Console.WriteLine("Item not found.");
                return;
            }

            Console.Write("Update Status (e.g., Found, Claimed, Lost): ");
            string newStatus = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(newStatus))
            {
                selectedItem.Status = newStatus;
                db.SaveChanges();
                ClearAndPrintHeader("Manage Lost Items");
                Console.WriteLine("Status updated successfully.");
            }
            else
            {
                ClearAndPrintHeader("Manage Lost Items");
                Console.WriteLine("Invalid status.");
            }
        }
    }
}
