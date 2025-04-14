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
    }
}