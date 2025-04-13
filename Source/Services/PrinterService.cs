using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Services
{
    public class PrinterService
    {
        //Method to print a table of items
        //Made it modular using c# reflection so that it can be used for an object of any class
        static void printTable<T>(List<T> itemsToPrint)
    {
        var type = typeof(T);
        //Get the properties of the type
        var properties = type.GetProperties();
        var columnWidth = new Dictionary<string, int>();

        //Calculate the width of each column based on the property name and value length
        foreach (var property in properties)
        {
            var maxLength = property.Name.Length;
            foreach (var item in itemsToPrint)
            {
                var value = property.GetValue(item)?.ToString() ?? string.Empty;
                if (value.Length > maxLength)
                {
                    maxLength = value.Length;
                }
            }
            columnWidth[property.Name] = maxLength;
        }

        // Build dynamic format string
        string format = "| " + string.Join(" | ", properties.Select(p => $"{{0,-{columnWidth[p.Name]}}}")) + " |";

        // Print header
        var headers = properties.Select(p => p.Name).ToArray();
        Console.WriteLine(format, headers);

        // Print separator
        int totalWidth = columnWidth.Values.Sum() + (3 * properties.Length) + 1;
        Console.WriteLine(new string('-', totalWidth));

        // Print rows
        foreach (var item in itemsToPrint)
        {
            var values = properties.Select(p => (p.GetValue(item)?.ToString()) ?? "").ToArray();
            Console.WriteLine(format, values);
        }

    }
    }
}