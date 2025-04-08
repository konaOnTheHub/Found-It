using Microsoft.EntityFrameworkCore;
using Source.Data;
using Microsoft.Extensions.Configuration;

class Program
{
    static void Main(string[] args)
    {
        using var db = new ApplicationDbContext();

    }
}
