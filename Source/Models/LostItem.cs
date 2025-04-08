using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Models
{
    public class LostItem
    {
        [Key]
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty; // Name of the lost item
        public string Description { get; set; } = string.Empty; // Description of the lost item
        public string Location { get; set; } = string.Empty; // Location where the item was lost
        public DateOnly DateLost { get; set; }
        public User? User { get; set; } // Navigation property to the User who lost the item
        public int UserId { get; set; } // Foreign key to the User table
        public string Status { get; set; } = string.Empty; // e.g., "Lost", "Found", "Claimed"
    }
}