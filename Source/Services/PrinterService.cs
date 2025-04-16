using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Models;

namespace Source.Services
{
    public class PrinterService
    {
        // Method to print the header with the logo and tagline
        public static void PrintHeader()
        {
           
            Console.WriteLine("============================================================");

            
            Console.WriteLine("███████╗ ██████╗ ██╗   ██╗███╗   ██╗██████╗     ██╗████████╗");
            Console.WriteLine("██╔════╝██╔═══██╗██║   ██║████╗  ██║██╔══██╗    ██║╚══██╔══╝");
            Console.WriteLine("█████╗  ██║   ██║██║   ██║██╔██╗ ██║██║  ██║    ██║   ██║   ");
            Console.WriteLine("██╔══╝  ██║   ██║██║   ██║██║╚██╗██║██║  ██║    ██║   ██║   ");
            Console.WriteLine("██║     ╚██████╔╝╚██████╔╝██║ ╚████║██████╔╝    ██║   ██║   ");
            Console.WriteLine("╚═╝      ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝╚═════╝     ╚═╝   ╚═╝   ");

            
            Console.WriteLine("============================================================");
            Console.WriteLine("         Helping you reunite with what matters.");

            // Prints the current date and time
            Console.WriteLine($"                  {DateTime.Now.ToString("f")}");

            
            Console.WriteLine("============================================================");

            // Welcome message
            Console.WriteLine("\nWelcome to 'Found It' - Your Lost & Found Management System!");
            Console.WriteLine("============================================================");
            Console.WriteLine();
            
        }

        
        //Method to print a table of found items
        public static void printFoundItem(List<FoundItem> items)
        {

            // Include header names to calculate max width
            var idHeader = "ID";
            var nameHeader = "Name";
            var descHeader = "Description";
            var dateHeader = "Date Found";
            var statusHeader = "Status";
            var claimsHeader = "Claims";

            // Calculate max lengths
            int idWidth = Math.Max(items.Max(i => i.FoundId.ToString().Length), idHeader.Length);
            int nameWidth = Math.Max(items.Max(i => i.Name.Length), nameHeader.Length);
            int descWidth = Math.Max(items.Max(i => i.Description.Length), descHeader.Length);
            int dateWidth = Math.Max(items.Max(i => i.DateFound.ToString("yyyy-MM-dd").Length), dateHeader.Length);
            int statusWidth = Math.Max(items.Max(i => i.Status.Length), statusHeader.Length);
            int claimsWidth = claimsHeader.Length; // Claims is a count, so we can use the header length

            // Build dynamic format string
            string format = $"| {{0,-{idWidth}}} | {{1,-{nameWidth}}} | {{2,-{descWidth}}} | {{3,-{dateWidth}}} | {{4,-{statusWidth}}} | {{5,-{claimsWidth}}} |";

            // Print header
            Console.WriteLine(format, idHeader, nameHeader, descHeader, dateHeader, statusHeader, claimsHeader);

            // Print separator
            Console.WriteLine(new string('-', idWidth + nameWidth + descWidth + dateWidth + statusWidth + claimsWidth + 19)); 

            // Print rows
            foreach (var item in items)
            {
                Console.WriteLine(format,
                    item.FoundId,
                    item.Name,
                    item.Description,
                    item.DateFound.ToString("yyyy-MM-dd"),
                    item.Status,
                    item.Claims.Count);

            }

        }
    }
}
