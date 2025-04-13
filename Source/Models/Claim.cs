using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Source.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; } // Primary key

        [ForeignKey("User")]
        public int UserId { get; set; } // Foreign key to User
        public User User { get; set; } // Navigation property to User

        [ForeignKey("FoundItem")]
        public int FoundItemId { get; set; } // Foreign key to FoundItem
        public FoundItem FoundItem { get; set; } // Navigation property to FoundItem

        public DateTime DateClaimed { get; set; } // Date when the claim was made
      
        [MaxLength(50)]
        public string Status { get; set; } = string.Empty; // Status of the claim 
    }
}
