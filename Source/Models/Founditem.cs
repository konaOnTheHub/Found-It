using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Source.Models
{
    public class FoundItem
    {
        [Key]
        public int FoundId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; // description of the item
        public DateOnly DateFound { get; set; }  // Date the item was found
        public string Status { get; set; } = string.Empty;

       
        public List<Claim> Claims { get; set; } = new List<Claim>();  // Navigation property to Claim table
    }
}
