using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty; // Name of the user
        public string Email { get; set; } = string.Empty; // Email of the user
        public string PasswordHash { get; set; } = string.Empty; // Hashed password of the user
        public string Role { get; set; } = string.Empty; // Role of the user (e.g., "Admin", "User")
        public List<LostItem> LostItems { get; set; } = new List<LostItem>(); // Navigation property to the LostItems associated with the user
        public List<Claim> Claims { get; set; } = new List<Claim>();  // Navigation property to Claim table
    }
}