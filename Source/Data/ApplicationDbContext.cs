using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Source.Models;

namespace Source.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } 
        public DbSet<LostItem> LostItems { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        optionsBuilder.UseSqlServer("Data Source=Bolyhos;Initial Catalog=lostFoundDB;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
        }

    }
}