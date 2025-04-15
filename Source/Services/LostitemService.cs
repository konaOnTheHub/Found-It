using System;
using Source.Data;
using Source.Models;

namespace Source.Services
{
    public static class LostItemService
    {
        public static void ReportLostItem(ApplicationDbContext db, User currentUser)
        {
            Console.WriteLine("--------------------------------------------\nReport Lost Item\n--------------------------------------------");

            Console.Write("Enter the name of the lost item: ");
            string name = Console.ReadLine();
            Console.Write("Enter a description: ");
            string description = Console.ReadLine();
            Console.Write("Enter the location where it was lost: ");
            string location = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(location))
            {
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

            Console.WriteLine("Lost item reported successfully.");
        }
    }
}
