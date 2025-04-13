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
        private readonly string _connectionString;
        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbSet<User> Users { get; set; } 
        public DbSet<LostItem> LostItems { get; set; } 
        public DbSet<FoundItem> FoundItems { get; set; }
        public DbSet<Claim> Claims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        optionsBuilder.UseSqlServer(_connectionString);
        }

    }
}