using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Source.Data;
using Source.Models;

namespace Source.Services
{
    public static class LostItemService
    {
        // Method to report a lost item
        public static void ReportLostItem(ApplicationDbContext db, User currentUser)
        {
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

            Console.WriteLine("Lost item reported successfully.");
        }

        // Method to manage and update lost items
        public static void ManageLostItems(ApplicationDbContext db)
        {
            Console.WriteLine("--------------------------------------------\nManage Lost Items\n--------------------------------------------");

            // Retrieve all lost items from the database including user details
            var lostItems = db.LostItems.Include(i => i.User).ToList();

            // Check if there are no lost items
            if (!lostItems.Any())
            {
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
            if (!int.TryParse(input, out int itemId)) return;

            // Retrieve the selected item
            var selectedItem = db.LostItems.FirstOrDefault(i => i.ItemId == itemId);
            if (selectedItem == null)
            {
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
                Console.WriteLine("Status updated successfully.");
            }
            else
            {
                // Handle invalid status input
                Console.WriteLine("Invalid status.");
            }
        }
    }
}
