using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            Console.WriteLine("\nSelect an option:\n1. Update Found Item Info\n2. Delete Found Item\n3. Manage Claims\n4. Back to Main Menu\n--------------------------------------------");
            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    //Update found item method PRANTEK
                    break;
                case "2":
                    //Delete found item method
                    DeleteFoundItem(db);
                    break;
                case "3":
                    //Manage claims method 
                    ManageClaims(db); 
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    ManageFoundItems(db);
                    break;
            }

        }
        public static void ViewFoundItems(ApplicationDbContext db, User userLogged)
        {
            Console.WriteLine("--------------------------------------------\nView Found Items\n--------------------------------------------");
            //Get all found items from the database
            var foundItems = db.FoundItems.ToList();
            if (foundItems.Count == 0)
            {
                Console.WriteLine("No found items available.\n");
                return;
            }
            //Print the found items in a table format
            PrinterService.printFoundItem(foundItems);
            Console.WriteLine("\n1. Claim Item\n2. Back to Main Menu");
            string option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    ClaimService.CreateClaim(db, userLogged);
                    break;
                case "2":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    ViewFoundItems(db, userLogged);
                    break;
            }

        }
        private static void DeleteFoundItem(ApplicationDbContext db)
        {
            Console.WriteLine("--------------------------------------------\nDelete Found Item\n--------------------------------------------");
            Console.Write("!WARNING This will delete all claims made on the item. Leave blank to cancel!\nEnter the ID of the found item you want to delete: ");
            string input = Console.ReadLine();
            //Check if input is empty
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("--------------------------------------------\nDelete operation cancelled.\n--------------------------------------------");
                return;
            }
            //Check if input is a number
            if (!int.TryParse(input, out _))
            {
                Console.WriteLine("--------------------------------------------\nInvalid input. Please enter a valid number.\n--------------------------------------------");
                return;
            }
            FoundItem? foundItem = db.FoundItems.Include(x => x.Claims).FirstOrDefault(x => x.FoundId == int.Parse(input));
            //Check if found item exists
            if (foundItem == null)
            {
                Console.WriteLine("--------------------------------------------\nFound item not found.\n--------------------------------------------");
                return;
            }
            db.FoundItems.Remove(foundItem);
            db.SaveChanges();
            Console.WriteLine("--------------------------------------------\nFound item and all its associated claims were deleted successfully.\n--------------------------------------------");
        }
         private static void ManageClaims(ApplicationDbContext db)
         {
              Console.WriteLine("--------------------------------------------\nManage Claims for Found Items\n--------------------------------------------");

              // Retrieve all claims with related FoundItem and User info
              var claims = db.Claims.Include(c => c.FoundItem).Include(c => c.User).ToList();

              if (claims.Count == 0)
              {
                  Console.WriteLine("There are no claims to manage.");
                  return;
              }

              // Display all claims
              foreach (var claim in claims)
              {
                  Console.WriteLine($"Claim ID: {claim.ClaimId} | Item: {claim.FoundItem?.Name} | Claimed By: {claim.User?.Name} | Status: {claim.Status}");
              }

              // Prompt admin to select a claim
              Console.WriteLine("Enter the Claim ID you want to update (or press Enter to cancel): ");
              string input = Console.ReadLine();

              if (string.IsNullOrWhiteSpace(input))
              {
                  Console.WriteLine("Operation cancelled.");
                  return;
              }

              // Validates input 
              if (!int.TryParse(input, out int claimId))
              {
                  Console.WriteLine("Invalid input.");
                  return;
              }
              var selectedClaim = db.Claims.Include(c => c.FoundItem).Include(c => c.User).FirstOrDefault(c => c.ClaimId == claimId);

              if (selectedClaim == null)
              {
                  Console.WriteLine("Claim not found.");
                  return;
              }

              // Display selected claim info
              Console.WriteLine($"Selected Claim for: {selectedClaim.FoundItem?.Name}, Claimed by: {selectedClaim.User?.Name}");
              Console.Write("Enter new status (e.g. Approved, Rejected, Pending): ");
              string newStatus = Console.ReadLine();

              // Validates input
              if (string.IsNullOrWhiteSpace(newStatus))
              {
                  Console.WriteLine("Invalid status.");
                  return;
              }

              // Update claim status 
              selectedClaim.Status = newStatus;
              db.SaveChanges();

              Console.WriteLine("Claim status updated successfully.");
          }

    }
}
