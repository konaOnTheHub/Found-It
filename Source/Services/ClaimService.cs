using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            FoundItem? foundItem = db.FoundItems.FirstOrDefault(x => x.FoundId == int.Parse(input));
            //Check if found item exists
            if (foundItem == null)
            {
                Console.WriteLine("--------------------------------------------\nFound item not found.\n--------------------------------------------");
                return;
            }
            //Check if the found item is already claimed
            if (foundItem.Status == "Claimed")
            {
                Console.WriteLine("--------------------------------------------\nFound item is already claimed.\n--------------------------------------------");
                return;
            }
            //Check if the user has already claimed the item
            Claim? existingClaim = db.Claims.FirstOrDefault(x => x.FoundId == foundItem.FoundId && x.UserId == userLogged.UserId);
            if (existingClaim != null)
            {
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
            Console.WriteLine("--------------------------------------------\nClaim created successfully.\n--------------------------------------------");
            return;


        }
        public static void ViewAndRevokeUserClaims(ApplicationDbContext db, User currentUser)
        {
            Console.WriteLine("--------------------------------------------\nMy Claims\n--------------------------------------------");

            // Fetch all claims by the user and include the related found item info
            var userClaims = db.Claims
            .Include(c => c.FoundItem)
            .Where(c => c.UserId == currentUser.UserId)
            .ToList();

            if (userClaims.Count == 0)
            {
                Console.WriteLine("You have not made any claims yet.");
                return;
            }

            // Displays info about the found item and  the claim status
            foreach (var claim in userClaims)
            {
                Console.WriteLine($"Claim ID: {claim.ClaimId} | Item Name: {claim.FoundItem?.Name} | Item Status: {claim.FoundItem?.Status}");
            }

            Console.Write("\nEnter the ID of a claim you want to revoke (or press Enter to skip): ");
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) return;

            if (!int.TryParse(input, out int claimId))
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            var claimToRemove = db.Claims.FirstOrDefault(c => c.ClaimId == claimId && c.UserId == currentUser.UserId);
            if (claimToRemove == null)
            {
                 Console.WriteLine("Claim not found or not authorized.");
                 return;
            }

            db.Claims.Remove(claimToRemove);
            db.SaveChanges();
            Console.WriteLine("Claim revoked successfully.");
        }

    }
}
