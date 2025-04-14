using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Source.Models;

namespace Source.Services
{
    public class PrinterService
    {
        //Method to print a table of found items
        public static void printFoundItem(List<FoundItem> items)
        {

            // Include header names to calculate max width
            var idHeader = "ID";
            var nameHeader = "Name";
            var descHeader = "Description";
            var dateHeader = "Date Found";
            var statusHeader = "Status";

            // Calculate max lengths
            int idWidth = Math.Max(items.Max(i => i.FoundId.ToString().Length), idHeader.Length);
            int nameWidth = Math.Max(items.Max(i => i.Name.Length), nameHeader.Length);
            int descWidth = Math.Max(items.Max(i => i.Description.Length), descHeader.Length);
            int dateWidth = Math.Max(items.Max(i => i.DateFound.ToString("yyyy-MM-dd").Length), dateHeader.Length);
            int statusWidth = Math.Max(items.Max(i => i.Status.Length), statusHeader.Length);

            // Build dynamic format string
            string format = $"| {{0,-{idWidth}}} | {{1,-{nameWidth}}} | {{2,-{descWidth}}} | {{3,-{dateWidth}}} | {{4,-{statusWidth}}} |";

            // Print header
            Console.WriteLine(format, idHeader, nameHeader, descHeader, dateHeader, statusHeader);

            // Print separator
            Console.WriteLine(new string('-', idWidth + nameWidth + descWidth + dateWidth + statusWidth)); 

            // Print rows
            foreach (var item in items)
            {
                Console.WriteLine(format,
                    item.FoundId,
                    item.Name,
                    item.Description,
                    item.DateFound.ToString("yyyy-MM-dd"),
                    item.Status);

            }

        }
    }
}