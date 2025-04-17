using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Source.Data;
using Source.Models;


namespace Source.Services
{
    public class ClaimService
    {
        public static void CreateClaim(ApplicationDbContext db, User userLogged)
        {

            Console.WriteLine("--------------------------------------------\nCreate Claim\n--------------------------------------------\n");
            Console.Write("Enter the ID of the found item you want to claim: ");
            string input = Console.ReadLine();
            //Check if input is convertable to an integer
            if (!int.TryParse(input, out int foundId))
            {
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("--------------------------------------------\nInvalid input. Please enter a valid ID.\n--------------------------------------------");
                return;
            }
            FoundItem? foundItem = db.FoundItems.FirstOrDefault(x => x.FoundId == int.Parse(input));
            //Check if found item exists
            if (foundItem == null)
            {
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("--------------------------------------------\nFound item not found.\n--------------------------------------------");
                return;
            }
            //Check if the found item is already claimed
            if (foundItem.Status == "Claimed")
            {
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("--------------------------------------------\nFound item is already claimed.\n--------------------------------------------");
                return;
            }
            //Check if the user has already claimed the item
            Claim? existingClaim = db.Claims.FirstOrDefault(x => x.FoundId == foundItem.FoundId && x.UserId == userLogged.UserId);
            if (existingClaim != null)
            {
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("--------------------------------------------\nYou have already claimed this item.\n--------------------------------------------");
                return;
            }
            //Create a new claim on the found item
            Claim claim = new Claim
            {
                UserId = userLogged.UserId,
                FoundId = foundItem.FoundId,
                DateClaimed = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending" // Default status for new claims
            };
            db.Claims.Add(claim);
            db.SaveChanges();
            Console.Clear();
            PrinterService.PrintHeader();
            Console.WriteLine("--------------------------------------------\nClaim created successfully.\n--------------------------------------------");
            return;


        }
        public static void ViewAndRevokeUserClaims(ApplicationDbContext db, User currentUser)
        {
            Console.Clear();
            PrinterService.PrintHeader();
            Console.WriteLine("--------------------------------------------\nMy Claims\n--------------------------------------------");

            // Fetch all claims by the user and include the related found item info
            var userClaims = db.Claims
            .Include(c => c.FoundItem)
            .Where(c => c.UserId == currentUser.UserId)
            .ToList();

            if (userClaims.Count == 0)
            {
                Console.Clear();
                PrinterService.PrintHeader();
                Console.WriteLine("You have not made any claims yet.");
                return;
            }

            // Displays info about the found item and  the claim status
            foreach (var claim in userClaims)
            {
                Console.WriteLine($"Claim ID: {claim.ClaimId} | Item Name: {claim.FoundItem?.Name} | Claim Status: {claim.Status}");
            }

            Console.Write("\n1. Revoke a claim\n2. Back to main menu\nSelect an option: ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    Console.Write("Enter the ID of the claim you want to revoke: ");
                    string inputId = Console.ReadLine();

                    if (!int.TryParse(inputId, out int claimId))
                    {
                        Console.Clear();
                        PrinterService.PrintHeader();
                        Console.WriteLine("Invalid input.");
                        return;
                    }

                    var claimToRemove = db.Claims.FirstOrDefault(c => c.ClaimId == claimId && c.UserId == currentUser.UserId);
                    if (claimToRemove == null)
                    {
                        Console.Clear();
                        PrinterService.PrintHeader();
                        Console.WriteLine("Claim not found or not authorized.");
                        return;
                    }
                    // Check if the claim is already approved
                    if (claimToRemove.Status == "Approved")
                    {
                        Console.Clear();
                        PrinterService.PrintHeader();
                        Console.WriteLine("Cannot revoke an approved claim.");
                        return;
                    }

                    db.Claims.Remove(claimToRemove);
                    db.SaveChanges();
                    Console.Clear();
                    PrinterService.PrintHeader();
                    Console.WriteLine("Claim revoked successfully.");
                    break;
                case "2":
                    Console.Clear();
                    PrinterService.PrintHeader();
                    return;
                default:
                    Console.Clear();
                    PrinterService.PrintHeader();
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }

        }

    }
}
