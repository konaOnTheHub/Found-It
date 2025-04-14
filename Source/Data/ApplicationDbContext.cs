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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Claim>()
                .HasOne(x => x.FoundItem)
                .WithMany(x => x.Claims)
                .HasForeignKey(x => x.FoundId)
                .OnDelete(DeleteBehavior.Cascade); //Ensures when a found Item is deleted it deletes all claims associated
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
            
        }

    }
}